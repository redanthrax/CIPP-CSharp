using CIPP.Api.Modules.Tenants.Commands;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Tenants.Endpoints;

public static class SyncTenantFromGraph {
    public static void MapSyncTenantFromGraph(this RouteGroupBuilder group) {
        group.MapPost("/{tenantId:guid}/sync", HandleAsync)
            .WithName("SyncTenantFromGraph")
            .WithSummary("Sync tenant information from Microsoft Graph")
            .WithDescription("Updates tenant information from Microsoft Graph API")
            .RequirePermission("Tenant.Write", "Sync tenant information from Microsoft Graph");
    }
    
    private static async Task<IResult> HandleAsync(
        IMediator mediator,
        Guid tenantId,
        CancellationToken cancellationToken = default) {
        try {
            var command = new SyncTenantFromGraphCommand(tenantId);
            var result = await mediator.Send(command, cancellationToken);
            
            return Results.Ok(Response<string>.SuccessResult(result));
        }
        catch (InvalidOperationException ex) {
            if (ex.Message.Contains("not found")) {
                return Results.NotFound(Response<string>.ErrorResult(ex.Message));
            }
            
            var errorDetails = ex.Data.Contains("ErrorDetails") 
                ? (List<string>)ex.Data["ErrorDetails"]! 
                : new List<string>();
            
            return Results.UnprocessableEntity(Response<string>.ErrorResult(ex.Message, errorDetails));
        }
        catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error syncing tenant from Microsoft Graph"
            );
        }
    }
}