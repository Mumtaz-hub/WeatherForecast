using Common.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Weather;

namespace Services.Installers
{
    public class CoreInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddScoped<IWeatherService, WeatherService>();
        }
    }
}
