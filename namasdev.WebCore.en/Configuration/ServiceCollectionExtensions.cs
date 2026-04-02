using Microsoft.Extensions.DependencyInjection;

namespace namasdev.WebCore.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNamasdevWeb(this IServiceCollection services,
            Action<WebCoreOptions>? configure = null)
        {
            var optionsBuilder = services.AddOptions<WebCoreOptions>();
            if (configure != null)
                optionsBuilder.Configure(configure);
            return services;
        }
    }
}
