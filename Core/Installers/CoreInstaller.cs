using Common.Interface;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Installers
{
    public class CoreInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
        }
    }
}
