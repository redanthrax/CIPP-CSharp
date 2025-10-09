using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Tenants.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Tenants;
using DispatchR;

namespace CIPP.Api.Modules.Tenants.Endpoints;

public static class CreateTenantGroup {
    public static void MapCreateTenantGroup(this RouteGroupBuilder group) {
        group.MapPost("/groups", Handle)
            .WithName("CreateTenantGroup")
            .WithSummary("Create tenant group")
            .WithDescription("Creates a new tenant group with optional member tenants")
            .RequirePermission("Tenant.Groups.Write", "Create and manage tenant groups");
    }

    private static async Task<IResult> Handle(
        CreateTenantGroupDto request,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new CreateTenantGroupCommand(
                request.Name,
                request.Description,
                request.MemberTenantIds
            );

            var tenantGroup = await mediator.Send(command, cancellationToken);

            var tenantGroupDto = new TenantGroupDto(
                tenantGroup.Id,
                tenantGroup.Name,
                tenantGroup.Description,
                tenantGroup.CreatedAt,
                tenantGroup.CreatedBy,
                tenantGroup.UpdatedAt,
                tenantGroup.UpdatedBy,
                request.MemberTenantIds
            );

            return Results.Ok(Response<TenantGroupDto>.SuccessResult(tenantGroupDto, "Tenant group created successfully"));
        } catch (InvalidOperationException ex) {
            return Results.BadRequest(Response<TenantGroupDto>.ErrorResult(ex.Message));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error creating tenant group"
            );
        }
    }
}