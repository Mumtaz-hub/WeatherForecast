using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Api.Extensions;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Data;
using Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Api
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        public ILifetimeScope ApplicationContainer { get; private set; }

        public Startup(IHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
                .AddJsonFile($"appsettings.overrides.json", true, true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
                builder.AddUserSecrets<Startup>();

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterServicesInAssembly(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ApplicationContainer = app.ApplicationServices.GetAutofacRoot();

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var database = serviceScope.ServiceProvider.GetService<DatabaseContext>();
                if (database is not null && !database.Database.IsInMemory() && !database.AllMigrationsApplied())
                {
                    database.Database.Migrate();
                }
            }

            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(_ => true) // allow any origin
                .AllowCredentials());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Weather Forecast");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseErrorHandlingMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
