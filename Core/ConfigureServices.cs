using Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core
{
    public static class ConfigureServices
    {

        public static void RegisterServicesInCore(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.RegisterServicesInAssembly(configuration);
        }
        
    }
}
