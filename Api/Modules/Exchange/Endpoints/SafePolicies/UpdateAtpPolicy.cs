using CIPP.Api.Extensions;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.SafePolicies;

public static class UpdateAtpPolicy {
    public static void MapUpdateAtpPolicy(this RouteGroupBuilder group) {
        group.MapPut("/", Handle)
            .WithName("UpdateAtpPolicy")
            .WithSummary("Update ATP policy")
            .WithDescription("Updates the ATP policy for Office 365")
            .RequirePermission("Exchange.Atp.Write", "Update ATP policy");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        string tenantId,
        UpdateAtpPolicyDto updateDto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new UpdateAtpPolicyCommand(tenantId, updateDto);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<object>.SuccessResult(null, "Update ATP policy successful"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error with update atp policy"
            );
        }
    }
}
