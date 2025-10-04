namespace CIPP.Api.Modules.Swagger;
public static class SwaggerExtensions
{
    public static void UseSwaggerModule(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                // Configure v1 endpoint
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "CIPP API V1");
                
                // OAuth configuration
                options.OAuthClientId(app.Configuration["Authentication:AzureAd:ClientId"]);
                options.OAuthScopes(app.Configuration["Authentication:AzureAd:Scope"]);
                options.OAuthUsePkce();
                options.OAuthAppName("CIPP API - Swagger UI");
            });
        }
    }
}
