using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Api
{ 
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .ConfigureAppConfiguration(config =>
                {
                    config
                        .AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile($"appsettings.overrides.json", true, true);
                })
                .UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                    .ReadFrom.Configuration(hostingContext.Configuration));
    }
}
