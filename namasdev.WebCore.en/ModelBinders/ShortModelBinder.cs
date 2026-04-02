using System.Globalization;

using Microsoft.AspNetCore.Mvc.ModelBinding;

using namasdev.Core.Validation;

namespace namasdev.WebCore.ModelBinders
{
    public class ShortModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (value == ValueProviderResult.None
                || string.IsNullOrWhiteSpace(value.FirstValue))
            {
                bindingContext.Result = bindingContext.ModelType.IsGenericType
                    ? ModelBindingResult.Success((short?)null)
                    : ModelBindingResult.Success(default(short));
                return Task.CompletedTask;
            }

            if (!short.TryParse(value.FirstValue, NumberStyles.Any, CultureInfo.CurrentUICulture, out short valor))
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, Validator.Messages.ShortInvalid(bindingContext.ModelName));
                bindingContext.Result = ModelBindingResult.Success(default(short));
                return Task.CompletedTask;
            }

            bindingContext.Result = ModelBindingResult.Success(valor);
            return Task.CompletedTask;
        }
    }

    public class ShortModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            return (context.Metadata.ModelType == typeof(short) || context.Metadata.ModelType == typeof(short?))
                ? new ShortModelBinder()
                : null;
        }
    }
}
