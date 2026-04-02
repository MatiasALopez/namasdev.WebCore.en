using System.Globalization;

using Microsoft.AspNetCore.Mvc.ModelBinding;

using namasdev.Core.Validation;

namespace namasdev.WebCore.ModelBinders
{
    public class DecimalModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (value == ValueProviderResult.None
                || string.IsNullOrWhiteSpace(value.FirstValue))
            {
                bindingContext.Result = bindingContext.ModelType.IsGenericType
                    ? ModelBindingResult.Success((decimal?)null)
                    : ModelBindingResult.Success(default(decimal));
                return Task.CompletedTask;
            }

            if (!decimal.TryParse(value.FirstValue, NumberStyles.Any, CultureInfo.CurrentUICulture, out decimal valor))
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, Validator.Messages.NumberInvalid(bindingContext.ModelName));
                bindingContext.Result = ModelBindingResult.Success(default(decimal));
                return Task.CompletedTask;
            }

            bindingContext.Result = ModelBindingResult.Success(valor);
            return Task.CompletedTask;
        }
    }

    public class DecimalModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            return (context.Metadata.ModelType == typeof(decimal) || context.Metadata.ModelType == typeof(decimal?))
                ? new DecimalModelBinder()
                : null;
        }
    }
}
