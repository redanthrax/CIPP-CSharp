using CIPP.Api.Modules.Microsoft.Services;
namespace CIPP.Api.Modules.Microsoft;
public class MicrosoftModule
{
    public void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IMicrosoftGraphService, MicrosoftGraphService>();
        services.AddHttpClient("Microsoft", client =>
        {
            client.BaseAddress = new Uri("https://graph.microsoft.com/");
            client.DefaultRequestHeaders.Add("User-Agent", "CIPP-API/1.0");
        });
    }
    public void ConfigureEndpoints(RouteGroupBuilder moduleGroup)
    {
        moduleGroup.MapGet("/test-graph", async (IMicrosoftGraphService graphService) =>
        {
            try
            {
                var organization = await graphService.GetOrganizationAsync();
                return Results.Ok(new
                {
                    Success = true,
                    OrganizationName = organization?.DisplayName ?? "Unknown",
                    Message = "Microsoft Graph connection successful"
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: 500,
                    title: "Microsoft Graph connection failed"
                );
            }
        })
        .WithName("TestMicrosoftGraph")
        .WithSummary("Test Microsoft Graph connectivity")
        .WithDescription("Tests the connection to Microsoft Graph and returns organization information")
        .RequireAuthorization("CombinedAuth");
        moduleGroup.MapGet("/users", async (
            IMicrosoftGraphService graphService,
            string? tenantId = null,
            string? filter = null,
            int? top = null,
            string? select = null) =>
        {
            try
            {
                var users = await graphService.GetUsersAsync(tenantId, filter, top, select);
                return Results.Ok(users);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: 500,
                    title: "Failed to retrieve users"
                );
            }
        })
        .WithName("GetUsers")
        .WithSummary("Get users from Microsoft Graph")
        .WithDescription("Retrieves users from Microsoft Graph with optional filtering")
        .RequireAuthorization("CombinedAuth");
        moduleGroup.MapGet("/users/{userId}", async (
            string userId,
            IMicrosoftGraphService graphService,
            string? tenantId = null) =>
        {
            try
            {
                var user = await graphService.GetUserAsync(userId, tenantId);
                if (user == null)
                {
                    return Results.NotFound($"User {userId} not found");
                }
                return Results.Ok(user);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: 500,
                    title: "Failed to retrieve user"
                );
            }
        })
        .WithName("GetUser")
        .WithSummary("Get a specific user from Microsoft Graph")
        .WithDescription("Retrieves a specific user by ID from Microsoft Graph")
        .RequireAuthorization("CombinedAuth");
        moduleGroup.MapGet("/groups", async (
            IMicrosoftGraphService graphService,
            string? tenantId = null,
            string? filter = null,
            int? top = null) =>
        {
            try
            {
                var groups = await graphService.GetGroupsAsync(tenantId, filter, top);
                return Results.Ok(groups);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: 500,
                    title: "Failed to retrieve groups"
                );
            }
        })
        .WithName("GetGroups")
        .WithSummary("Get groups from Microsoft Graph")
        .WithDescription("Retrieves groups from Microsoft Graph with optional filtering")
        .RequireAuthorization("CombinedAuth");
        moduleGroup.MapGet("/devices", async (
            IMicrosoftGraphService graphService,
            string? tenantId = null,
            string? filter = null,
            int? top = null) =>
        {
            try
            {
                var devices = await graphService.GetDevicesAsync(tenantId, filter, top);
                return Results.Ok(devices);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: 500,
                    title: "Failed to retrieve devices"
                );
            }
        })
        .WithName("GetDevices")
        .WithSummary("Get devices from Microsoft Graph")
        .WithDescription("Retrieves devices from Microsoft Graph with optional filtering")
        .RequireAuthorization("CombinedAuth");
        
        // Partner tenant endpoints
        moduleGroup.MapGet("/partner-tenants", async (
            IMicrosoftGraphService graphService,
            string? filter = null,
            int? top = null,
            int? skip = null) =>
        {
            try
            {
                var tenants = await graphService.GetPartnerTenantsAsync(filter, top, skip);
                return Results.Ok(tenants);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: 500,
                    title: "Failed to retrieve partner tenants"
                );
            }
        })
        .WithName("GetPartnerTenants")
        .WithSummary("Get partner tenants from Microsoft Graph")
        .WithDescription("Retrieves all partner/customer tenant relationships from Microsoft Graph")
        .RequireAuthorization("CombinedAuth");
        
        moduleGroup.MapGet("/partner-tenants/{contractId}", async (
            string contractId,
            IMicrosoftGraphService graphService) =>
        {
            try
            {
                var tenant = await graphService.GetPartnerTenantAsync(contractId);
                if (tenant == null)
                {
                    return Results.NotFound($"Partner tenant {contractId} not found");
                }
                return Results.Ok(tenant);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: 500,
                    title: "Failed to retrieve partner tenant"
                );
            }
        })
        .WithName("GetPartnerTenant")
        .WithSummary("Get a specific partner tenant from Microsoft Graph")
        .WithDescription("Retrieves a specific partner tenant by contract ID from Microsoft Graph")
        .RequireAuthorization("CombinedAuth");
        
        moduleGroup.MapGet("/tenants/{tenantId}/domains", async (
            string tenantId,
            IMicrosoftGraphService graphService,
            string? filter = null) =>
        {
            try
            {
                var domains = await graphService.GetTenantDomainsAsync(tenantId, filter);
                return Results.Ok(domains);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: 500,
                    title: "Failed to retrieve tenant domains"
                );
            }
        })
        .WithName("GetTenantDomains")
        .WithSummary("Get domains for a specific tenant")
        .WithDescription("Retrieves all domains for a specific tenant from Microsoft Graph")
        .RequireAuthorization("CombinedAuth");
    }
}
