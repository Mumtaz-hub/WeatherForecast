using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Extensions;

namespace Services
{
    public static class ConfigureServices
    {
        public static void RegisterServicesInServices(this IServiceCollection services,  IConfigurationRoot configuration)
        {
            services.RegisterServicesInAssembly(configuration);
        }
    }
}
