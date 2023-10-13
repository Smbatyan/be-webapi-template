using BaseConverter;
using Microsoft.OpenApi.Models;
using WebApi.Extensions.Health;

namespace WebApi.Extensions;

public static class SwaggerExtension
{
    public static IServiceCollection
        AddPandaSwaggerGen(this IServiceCollection services, IConfiguration configuration) // Move to common
    {
        services.AddSwaggerGen(options =>
        {
            SwaggerOptions swaggerOptions =
                configuration.GetSection("Swagger").Get<SwaggerOptions>() ?? SwaggerOptions.Default;

            foreach (SwaggerVersionOptions version in swaggerOptions.Versions)
            {
                options.SwaggerDoc(version.Key, new OpenApiInfo
                {
                    Title = version.Title,
                    Description = version.Description,
                    //  "Powered by PandaTech LLC: Where precision meets innovation. Let's build the future, one endpoint at a time.",
                    Contact = new OpenApiContact
                    {
                        Name = "PandaTech LLC",
                        Email = "info@pandatech.it",
                        Url = new Uri("https://pandatech.it"),
                    }
                });
            }


            //This option is created because due to some bug /health endpoint is not working in .NET 7. It's included in Microsoft planning.
            options.DocumentFilter<HealthChecksFilter>();

            // Add string input support into int64 field
            options.ParameterFilter<PandaParameterBaseConverter>();

            // Add the token authentication option
            options.AddSecurityDefinition("token", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Name = "token",
                Description = "Token authentication using the bearer scheme"
            });

            // Require the token to be passed as a header for API calls
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "token"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }

    public static void UsePandaSwagger(this WebApplication app, IConfiguration configuration)
    {
        if (app.Environment.IsProduction()) return;

        SwaggerOptions swaggerOptions =
            configuration.GetSection("Swagger").Get<SwaggerOptions>() ?? SwaggerOptions.Default;

        app.UseSwagger(c =>
        {
            c.RouteTemplate = "swagger/{documentName}/swagger.json";
            c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
            {
                swaggerDoc.Servers = new List<OpenApiServer>
                {
                    new()
                    {
                        Url =
                            $"https://{httpReq.Host.Value}{swaggerOptions.ApiBasePath}"
                    }
                };
            });
        });
        app.UseSwaggerUI(c =>
        {
            foreach (SwaggerVersionOptions version in swaggerOptions.Versions)
            {
                c.SwaggerEndpoint($"{swaggerOptions.JsonRoutePrefix}/swagger/{version.Key}/swagger.json",
                    version.Title);
            }

            c.RoutePrefix = "swagger";
        });
        app.UseStaticFiles();

        app.UseSwaggerUI(options =>
        {
            options.InjectStylesheet("/assets/css/panda-style.css");
            options.InjectJavascript("/assets/js/docs.js");
        });
    } // move to common

    public class SwaggerOptions
    {
        public static SwaggerOptions Default = new SwaggerOptions()
        {
            Enabled = false,
            JsonRoutePrefix = "",
            ApiBasePath = "/",
            ApiBaseScheme = "http",
            Versions = new()
            {
                new SwaggerVersionOptions()
                {
                    Key = "v1",
                    Title = "API V1",
                    Description = "API V1"
                }
            }
        };

        public bool Enabled { get; set; }

        public string JsonRoutePrefix { get; set; }

        public string ApiBasePath { get; set; }

        public string ApiBaseScheme { get; set; }

        public List<SwaggerVersionOptions> Versions { get; set; }
    } // move to common

    public class SwaggerVersionOptions
    {
        public string Key { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    } // move to common
}