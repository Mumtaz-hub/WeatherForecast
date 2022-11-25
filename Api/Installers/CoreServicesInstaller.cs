using Ardalis.GuardClauses;
using Common;
using Common.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace Api.Installers
{
    public class CoreServicesInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            Guard.Against.Null(services, nameof(services));

            services.AddHttpClient();
            services.AddCors();
            services.AddControllers();
            services.AddSwaggerGenNewtonsoftSupport();
            services.AddSingleton(configuration);
            services.AddLogging();
            
            AddSerializerSettings(services);
            AddAppSettings(services, configuration);
            AddWeatherApiSettings(services, configuration);
        }

        private static void AddSerializerSettings(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson(options => { options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); });
        }

        private static void AddAppSettings(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration.GetSection(AppSettings.Key));
        }

        private static void AddWeatherApiSettings(IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<WeatherApiSettings>()
                .Bind(configuration.GetSection(WeatherApiSettings.Key))
                .ValidateDataAnnotations();
        }
    }
}
