using Microsoft.OpenApi.Models;
using CIPP.Api.Extensions;
namespace CIPP.Api.Modules.Swagger;
public class SwaggerModule : IVersioningExcluded
{
    public void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        using var serviceProvider = services.BuildServiceProvider();
        var environment = serviceProvider.GetService<IWebHostEnvironment>();
        if (environment == null || !environment.IsDevelopment())
        {
            return;
        }
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            var tenantId = configuration["Authentication:AzureAd:TenantId"];
            var clientId = configuration["Authentication:AzureAd:ClientId"];
            var scope = configuration["Authentication:AzureAd:Scope"] ?? "api://default";
            
            options.AddSecurityDefinition("EntraId", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/authorize"),
                        TokenUrl = new Uri($"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token"),
                        Scopes = new Dictionary<string, string>
                        {
                            { scope, "Access the API" }
                        }
                    }
                },
                Description = "Entra ID OAuth2 Authentication (Authorization Code Flow with PKCE)"
            });
            options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Name = "X-API-Key",
                Description = "API Key Authentication"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "EntraId"
                        }
                    },
                    new[] { scope }
                },
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "ApiKey"
                        }
                    },
                    Array.Empty<string>()
                }
            });
            
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "CIPP API",
                Version = "v1",
                Description = "Cyberdrain Improved Partner Portal API"
            });
            
            options.DocInclusionPredicate((version, apiDesc) =>
            {
                if (apiDesc.ActionDescriptor.EndpointMetadata.OfType<InternalAttribute>().Any())
                    return false;
                    
                if (apiDesc.GroupName == null) 
                    return version == "v1";
                    
                return apiDesc.GroupName == version;
            });
        });
    }
    public void ConfigureEndpoints(RouteGroupBuilder moduleGroup)
    {
    }
}
