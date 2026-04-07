using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;

using namasdev.WebCore.ModelBinders;

namespace namasdev.WebCore.Tests.ModelBinders
{
    /// <summary>
    /// Minimal IValueProvider that returns a single named value.
    /// </summary>
    internal class SingleValueProvider : IValueProvider
    {
        private readonly string _key;
        private readonly string? _value;

        public SingleValueProvider(string key, string? value)
        {
            _key = key;
            _value = value;
        }

        public bool ContainsPrefix(string prefix) => prefix == _key;

        public ValueProviderResult GetValue(string key)
            => key == _key && _value is not null
                ? new ValueProviderResult(new StringValues(_value))
                : ValueProviderResult.None;
    }

    internal static class BindingContextFactory
    {
        private static readonly EmptyModelMetadataProvider _metadataProvider = new();

        public static DefaultModelBindingContext Create(string modelName, Type modelType, string? rawValue)
        {
            var metadata = _metadataProvider.GetMetadataForType(modelType);
            return new DefaultModelBindingContext
            {
                ModelName = modelName,
                ModelMetadata = metadata,
                ModelState = new ModelStateDictionary(),
                ValueProvider = new SingleValueProvider(modelName, rawValue),
            };
        }
    }

    // ── IntegerModelBinder ────────────────────────────────────────────────────

    public class IntegerModelBinderTests
    {
        private static DefaultModelBindingContext Ctx(string? value, Type? type = null)
            => BindingContextFactory.Create("val", type ?? typeof(int), value);

        [Fact]
        public async Task BindModelAsync_ValidInt_SetsResult()
        {
            var ctx = Ctx("42");
            await new IntegerModelBinder().BindModelAsync(ctx);
            Assert.Equal(ModelBindingResult.Success(42), ctx.Result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task BindModelAsync_EmptyValue_NonNullable_DefaultsToZero(string? input)
        {
            var ctx = Ctx(input, typeof(int));
            await new IntegerModelBinder().BindModelAsync(ctx);
            Assert.Equal(ModelBindingResult.Success(0), ctx.Result);
        }

        [Fact]
        public async Task BindModelAsync_EmptyValue_Nullable_DefaultsToNull()
        {
            var ctx = Ctx(null, typeof(int?));
            await new IntegerModelBinder().BindModelAsync(ctx);
            Assert.Equal(ModelBindingResult.Success((int?)null), ctx.Result);
        }

        [Fact]
        public async Task BindModelAsync_InvalidValue_AddsModelError()
        {
            var ctx = Ctx("notanumber");
            await new IntegerModelBinder().BindModelAsync(ctx);
            Assert.False(ctx.ModelState.IsValid);
            Assert.True(ctx.ModelState.ContainsKey("val"));
        }
    }

    // ── DecimalModelBinder ────────────────────────────────────────────────────

    public class DecimalModelBinderTests
    {
        private static DefaultModelBindingContext Ctx(string? value, Type? type = null)
            => BindingContextFactory.Create("val", type ?? typeof(decimal), value);

        [Fact]
        public async Task BindModelAsync_ValidDecimal_SetsResult()
        {
            // Use current UI culture's decimal separator to stay culture-agnostic.
            string input = string.Format(System.Globalization.CultureInfo.CurrentUICulture, "{0}", 3.14m);
            var ctx = Ctx(input);
            await new DecimalModelBinder().BindModelAsync(ctx);
            Assert.Equal(ModelBindingResult.Success(3.14m), ctx.Result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task BindModelAsync_EmptyValue_NonNullable_DefaultsToZero(string? input)
        {
            var ctx = Ctx(input, typeof(decimal));
            await new DecimalModelBinder().BindModelAsync(ctx);
            Assert.Equal(ModelBindingResult.Success(default(decimal)), ctx.Result);
        }

        [Fact]
        public async Task BindModelAsync_EmptyValue_Nullable_DefaultsToNull()
        {
            var ctx = Ctx(null, typeof(decimal?));
            await new DecimalModelBinder().BindModelAsync(ctx);
            Assert.Equal(ModelBindingResult.Success((decimal?)null), ctx.Result);
        }

        [Fact]
        public async Task BindModelAsync_InvalidValue_AddsModelError()
        {
            var ctx = Ctx("abc");
            await new DecimalModelBinder().BindModelAsync(ctx);
            Assert.False(ctx.ModelState.IsValid);
            Assert.True(ctx.ModelState.ContainsKey("val"));
        }
    }

    // ── DateTimeModelBinder ───────────────────────────────────────────────────

    public class DateTimeModelBinderTests
    {
        private static DefaultModelBindingContext Ctx(string? value, Type? type = null)
            => BindingContextFactory.Create("val", type ?? typeof(DateTime), value);

        [Fact]
        public async Task BindModelAsync_ValidDate_SetsResult()
        {
            var ctx = Ctx("2024-06-15");
            await new DateTimeModelBinder().BindModelAsync(ctx);
            var dt = (DateTime)ctx.Result.Model!;
            Assert.Equal(new DateTime(2024, 6, 15), dt);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("1234567")] // < 8 chars
        public async Task BindModelAsync_EmptyOrShortValue_NonNullable_DefaultsToMinValue(string? input)
        {
            var ctx = Ctx(input, typeof(DateTime));
            await new DateTimeModelBinder().BindModelAsync(ctx);
            Assert.Equal(ModelBindingResult.Success(default(DateTime)), ctx.Result);
        }

        [Fact]
        public async Task BindModelAsync_EmptyValue_Nullable_DefaultsToNull()
        {
            var ctx = Ctx(null, typeof(DateTime?));
            await new DateTimeModelBinder().BindModelAsync(ctx);
            Assert.Equal(ModelBindingResult.Success((DateTime?)null), ctx.Result);
        }

        [Fact]
        public async Task BindModelAsync_InvalidValue_AddsModelError()
        {
            var ctx = Ctx("not-a-date");
            await new DateTimeModelBinder().BindModelAsync(ctx);
            Assert.False(ctx.ModelState.IsValid);
            Assert.True(ctx.ModelState.ContainsKey("val"));
        }

        [Fact]
        public async Task BindModelAsync_DateWithTime_ParsesDatePart()
        {
            var ctx = Ctx("2024-06-15 14:30:00");
            await new DateTimeModelBinder().BindModelAsync(ctx);
            var dt = (DateTime)ctx.Result.Model!;
            Assert.Equal(new DateTime(2024, 6, 15), dt.Date);
        }
    }
}
