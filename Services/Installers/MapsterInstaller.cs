using System.Reflection;
using Common.Interface;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Services.Installers
{
    public class MapsterInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());
            var mapperConfig = new Mapper(config);
            services.AddSingleton<IMapper>(mapperConfig);
        }
    }
}
