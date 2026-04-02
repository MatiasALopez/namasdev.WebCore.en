using Microsoft.AspNetCore.Mvc;
using namasdev.WebCore.ModelBinders;

namespace namasdev.WebCore.Configuration
{
    public static class MvcOptionsExtensions
    {
        public static void AddNamasdevWebModelBinders(this MvcOptions options)
        {
            options.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider());
            options.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());
            options.ModelBinderProviders.Insert(0, new DoubleModelBinderProvider());
            options.ModelBinderProviders.Insert(0, new IntegerModelBinderProvider());
            options.ModelBinderProviders.Insert(0, new LongModelBinderProvider());
            options.ModelBinderProviders.Insert(0, new ShortModelBinderProvider());
        }
    }
}
