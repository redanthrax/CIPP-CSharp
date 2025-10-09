using CIPP.Api.Modules.Tenants.Commands;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Tenants.Endpoints;

public static class DeleteTenantGroup
{
    public static void MapDeleteTenantGroup(this RouteGroupBuilder group)
    {
        group.MapDelete("/groups/{groupId:guid}", Handle)
            .WithName("DeleteTenantGroup")
            .WithSummary("Delete a tenant group")
            .WithDescription("Deletes a tenant group and all its memberships")
            .RequirePermission("Tenant.Groups.Delete", "Delete tenant groups and their memberships");
    }

    private static async Task<IResult> Handle(
        Guid groupId,
        IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = new DeleteTenantGroupCommand(groupId);
            var success = await mediator.Send(command, cancellationToken);
            
            if (success)
            {
                return Results.Ok(Response<bool>.SuccessResult(true));
            }
            else
            {
                return Results.NotFound(Response<bool>.ErrorResult("Tenant group not found"));
            }
        }
        catch (Exception ex)
        {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error deleting tenant group"
            );
        }
    }
}