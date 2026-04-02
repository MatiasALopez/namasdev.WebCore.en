using System.Globalization;

using Microsoft.AspNetCore.Mvc.ModelBinding;

using namasdev.Core.Validation;

namespace namasdev.WebCore.ModelBinders
{
    public class LongModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (value == ValueProviderResult.None
                || string.IsNullOrWhiteSpace(value.FirstValue))
            {
                bindingContext.Result = bindingContext.ModelType.IsGenericType
                    ? ModelBindingResult.Success((long?)null)
                    : ModelBindingResult.Success(default(long));
                return Task.CompletedTask;
            }

            if (!long.TryParse(value.FirstValue, NumberStyles.Any, CultureInfo.CurrentUICulture, out long valor))
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, Validator.Messages.LongInvalid(bindingContext.ModelName));
                bindingContext.Result = ModelBindingResult.Success(default(long));
                return Task.CompletedTask;
            }

            bindingContext.Result = ModelBindingResult.Success(valor);
            return Task.CompletedTask;
        }
    }

    public class LongModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            return (context.Metadata.ModelType == typeof(long) || context.Metadata.ModelType == typeof(long?))
                ? new LongModelBinder()
                : null;
        }
    }
}
