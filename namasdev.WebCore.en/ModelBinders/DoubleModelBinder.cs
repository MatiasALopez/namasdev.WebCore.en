using System.Globalization;

using Microsoft.AspNetCore.Mvc.ModelBinding;

using namasdev.Core.Validation;

namespace namasdev.WebCore.ModelBinders
{
    public class DoubleModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (value == ValueProviderResult.None
                || string.IsNullOrWhiteSpace(value.FirstValue))
            {
                bindingContext.Result = bindingContext.ModelType.IsGenericType
                    ? ModelBindingResult.Success((double?)null)
                    : ModelBindingResult.Success(default(double));
                return Task.CompletedTask;
            }

            if (!double.TryParse(value.FirstValue, NumberStyles.Any, CultureInfo.CurrentUICulture, out double valor))
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, Validator.Messages.NumberInvalid(bindingContext.ModelName));
                bindingContext.Result = ModelBindingResult.Success(default(double));
                return Task.CompletedTask;
            }

            bindingContext.Result = ModelBindingResult.Success(valor);
            return Task.CompletedTask;
        }
    }

    public class DoubleModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            return (context.Metadata.ModelType == typeof(double) || context.Metadata.ModelType == typeof(double?))
                ? new DoubleModelBinder()
                : null;
        }
    }
}
