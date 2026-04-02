using System.Globalization;

using Microsoft.AspNetCore.Mvc.ModelBinding;

using namasdev.Core.Validation;

namespace namasdev.WebCore.ModelBinders
{
    public class IntegerModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (value == ValueProviderResult.None
                || string.IsNullOrWhiteSpace(value.FirstValue))
            {
                bindingContext.Result = bindingContext.ModelType.IsGenericType
                    ? ModelBindingResult.Success((int?)null)
                    : ModelBindingResult.Success(default(int));
                return Task.CompletedTask;
            }

            if (!int.TryParse(value.FirstValue, NumberStyles.Any, CultureInfo.CurrentUICulture, out int valor))
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, Validator.Messages.IntegerInvalid(bindingContext.ModelName));
                bindingContext.Result = ModelBindingResult.Success(default(int));
                return Task.CompletedTask;
            }

            bindingContext.Result = ModelBindingResult.Success(valor);
            return Task.CompletedTask;
        }
    }

    public class IntegerModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            return (context.Metadata.ModelType == typeof(int) || context.Metadata.ModelType == typeof(int?))
                ? new IntegerModelBinder()
                : null;
        }
    }
}
