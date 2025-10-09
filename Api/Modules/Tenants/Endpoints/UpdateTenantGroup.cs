using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Tenants.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Tenants;
using DispatchR;

namespace CIPP.Api.Modules.Tenants.Endpoints;

public static class UpdateTenantGroup {
    public static void MapUpdateTenantGroup(this RouteGroupBuilder group) {
        group.MapPut("/groups/{groupId:guid}", Handle)
            .WithName("UpdateTenantGroup")
            .WithSummary("Update a tenant group")
            .WithDescription("Updates an existing tenant group with new details")
            .RequirePermission("Tenant.Groups.Write", "Update tenant groups");
    }

    private static async Task<IResult> Handle(
        Guid groupId,
        CreateTenantGroupDto request,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new UpdateTenantGroupCommand(
                groupId,
                request.Name,
                request.Description,
                request.MemberTenantIds
            );

            var result = await mediator.Send(command, cancellationToken);

            var tenantGroupDto = new TenantGroupDto(
                result.Id,
                result.Name,
                result.Description,
                result.CreatedAt,
                result.CreatedBy,
                result.UpdatedAt,
                result.UpdatedBy,
                result.Memberships.Select(m => m.TenantId).ToList()
            );

            return Results.Ok(Response<TenantGroupDto>.SuccessResult(tenantGroupDto));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error updating tenant group"
            );
        }
    }
}