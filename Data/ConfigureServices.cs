using Data.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data
{
    public static class ConfigureServices
    {
        public static void RegisterServicesInData(this IServiceCollection services,  IConfigurationRoot configuration)
        {
            services.RegisterServicesInAssembly(configuration);
        }
    }
}
