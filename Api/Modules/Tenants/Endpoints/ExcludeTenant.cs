using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Tenants.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Tenants;
using DispatchR;

namespace CIPP.Api.Modules.Tenants.Endpoints;

public static class ExcludeTenant {
    public static void MapExcludeTenant(this RouteGroupBuilder group) {
        group.MapPost("/exclude", Handle)
            .WithName("ExcludeTenant")
            .WithSummary("Exclude/Include tenants")
            .WithDescription("Add or remove tenant exclusions from CIPP operations")
            .RequirePermission("Tenant.Write", "Exclude or include tenants from CIPP operations");
    }

    private static async Task<IResult> Handle(
        TenantExclusionDto request,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new ExcludeTenantCommand(
                request.TenantIds,
                request.AddExclusion
            );

            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult(result));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error updating tenant exclusions"
            );
        }
    }
}