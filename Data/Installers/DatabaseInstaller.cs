using System;
using Common.Extensions;
using Common.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Installers
{
    public class DatabaseInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            if (configuration["AppSettings:UseInMemoryDatabase"] is null)
                throw new ArgumentNullException("AppSettings:UseInMemoryDatabase settings missing");

            var useInMemoryDatabase = configuration["AppSettings:UseInMemoryDatabase"].ToBoolean();

            if (useInMemoryDatabase)
            {
                services.AddDbContext<DatabaseContext>(options =>
                {
                    options.UseInMemoryDatabase(Guid.NewGuid().ToString());
                });
                return;
            }

            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));
        }
    }
}
