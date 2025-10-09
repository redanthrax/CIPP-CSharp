using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Tenants.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Tenants;
using DispatchR;

namespace CIPP.Api.Modules.Tenants.Endpoints;

public static class GetTenantGroup {
    public static void MapGetTenantGroup(this RouteGroupBuilder group) {
        group.MapGet("/groups/{groupId:guid}", Handle)
            .WithName("GetTenantGroup")
            .WithSummary("Get a specific tenant group")
            .WithDescription("Returns a specific tenant group by ID with its members")
            .RequirePermission("Tenant.Groups.Read", "View tenant group details");
    }

    private static async Task<IResult> Handle(
        Guid groupId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetTenantGroupsQuery(groupId);
            var tenantGroups = await mediator.Send(query, cancellationToken);

            var tenantGroup = tenantGroups.FirstOrDefault();
            if (tenantGroup == null) {
                return Results.NotFound($"Tenant group with ID {groupId} not found");
            }

            var tenantGroupDto = new TenantGroupDto(
                tenantGroup.Id,
                tenantGroup.Name,
                tenantGroup.Description,
                tenantGroup.CreatedAt,
                tenantGroup.CreatedBy,
                tenantGroup.UpdatedAt,
                tenantGroup.UpdatedBy,
                tenantGroup.Memberships.Select(m => m.TenantId).ToList()
            );

            return Results.Ok(Response<TenantGroupDto>.SuccessResult(tenantGroupDto));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving tenant group"
            );
        }
    }
}