using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Tenants.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Tenants;
using DispatchR;

namespace CIPP.Api.Modules.Tenants.Endpoints;

public static class UpdateTenant {
    public static void MapUpdateTenant(this RouteGroupBuilder group) {
        group.MapPut("/{tenantId:guid}", Handle)
            .WithName("UpdateTenant")
            .WithSummary("Update tenant")
            .WithDescription("Updates tenant properties like alias and group memberships")
            .RequirePermission("Tenant.Write", "Update tenant properties and group memberships");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        UpdateTenantDto request,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new UpdateTenantCommand(
                tenantId,
                request.TenantAlias,
                request.TenantGroups
            );

            var tenant = await mediator.Send(command, cancellationToken);

            var tenantDto = new TenantDto(
                tenant.TenantId,
                tenant.DisplayName,
                tenant.DefaultDomainName,
                tenant.Status,
                tenant.CreatedAt,
                tenant.CreatedBy,
                tenant.Metadata
            );

            return Results.Ok(Response<TenantDto>.SuccessResult(tenantDto, "Tenant updated successfully"));
        } catch (InvalidOperationException) {
            return Results.NotFound(Response<TenantDto>.ErrorResult($"Tenant with TenantId {tenantId} not found"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error updating tenant"
            );
        }
    }
}