using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Interface
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollection services, IConfigurationRoot configuration);
    }
}
