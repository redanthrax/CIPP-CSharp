using Asp.Versioning;

namespace CIPP.Api.Modules.Versioning;

public class VersioningModule {
    public void RegisterServices(IServiceCollection services, IConfiguration configuration) {
        services.AddApiVersioning(options => {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new QueryStringApiVersionReader("version"),
                new HeaderApiVersionReader("X-Version")
            );
            options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
        });
    }

    public void ConfigureEndpoints(RouteGroupBuilder moduleGroup) {
        // This module doesn't need to register any endpoints
        // It only configures the versioning services
    }
}