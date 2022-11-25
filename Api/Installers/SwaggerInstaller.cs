using System;
using System.Collections.Generic;
using Common.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Api.Installers
{
    public class SwaggerInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            AddSwaggerSetup(services);
        }

        private static void AddSwaggerSetup(IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddSwaggerGen(ops =>
            {
                ops.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Weather forecast",
                    Version = "v1",
                    Description = "Weather forecast Api",
                    Contact = new OpenApiContact
                    {
                        Name = "AppsFactory",
                        Email = "info@appsfactory.de"
                    }
                });

                ops.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });

                ops.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            },
                            Scheme = "oauth2",
                            Name = JwtBearerDefaults.AuthenticationScheme,
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });
        }
    }
}
