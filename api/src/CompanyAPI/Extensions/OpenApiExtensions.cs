using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;

namespace Application.Extensions;

public static class OpenApiExtensions
{
    public static void AddOpenApi(IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Info = new OpenApiInfo
                    {
                        Title = "Company API",
                        Version = "v1",
                        Description = "API documentation with JWT Bearer authentication"
                    };

                    document.Components ??= new OpenApiComponents();
                    document.Components.SecuritySchemes = new Dictionary<string, OpenApiSecurityScheme>
                    {
                        ["Bearer"] = new OpenApiSecurityScheme
                        {
                            Type = SecuritySchemeType.Http,
                            Scheme = "bearer",
                            BearerFormat = "JWT",
                            Description = "Enter your JWT token in the format: Bearer {your token}"
                        }
                    };

                    document.SecurityRequirements = new List<OpenApiSecurityRequirement>
                    {
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                }
                    };

                    return Task.CompletedTask;
                });
        });
    }

    public static void MapOpenApi(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();

            app.MapScalarApiReference(options =>
            {
                options.Title = "Company API Documentation";
                options.Theme = ScalarTheme.Purple;
                options.ShowSidebar = true;
                options.Authentication = new ScalarAuthenticationOptions
                {
                    PreferredSecuritySchemes = new List<string> { "Bearer" },
                };
            });
        }
    }
}
