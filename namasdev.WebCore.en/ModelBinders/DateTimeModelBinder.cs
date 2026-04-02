using System.Globalization;

using Microsoft.AspNetCore.Mvc.ModelBinding;

using namasdev.Core.Validation;

namespace namasdev.WebCore.ModelBinders
{
    public class DateTimeModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (value == ValueProviderResult.None
                || string.IsNullOrWhiteSpace(value.FirstValue)
                || value.FirstValue.Length < 8)
            {
                bindingContext.Result = bindingContext.ModelType.IsGenericType
                    ? ModelBindingResult.Success((DateTime?)null)
                    : ModelBindingResult.Success(default(DateTime));
                return Task.CompletedTask;
            }

            if (!DateTime.TryParse(value.FirstValue.Split(' ')[0], CultureInfo.CurrentUICulture, DateTimeStyles.None, out DateTime dateTime))
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, Validator.Messages.DateTimeInvalid(bindingContext.ModelName));
                bindingContext.Result = ModelBindingResult.Success(default(DateTime));
                return Task.CompletedTask;
            }

            bindingContext.Result = ModelBindingResult.Success(dateTime);
            return Task.CompletedTask;
        }
    }

    public class DateTimeModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            return (context.Metadata.ModelType == typeof(DateTime) || context.Metadata.ModelType == typeof(DateTime?))
                ? new DateTimeModelBinder()
                : null;
        }
    }
}
