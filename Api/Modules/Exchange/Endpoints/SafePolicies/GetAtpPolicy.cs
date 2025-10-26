using CIPP.Api.Extensions;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.SafePolicies;

public static class GetAtpPolicy {
    public static void MapGetAtpPolicy(this RouteGroupBuilder group) {
        group.MapGet("/", Handle)
            .WithName("GetAtpPolicy")
            .WithSummary("Get ATP policy")
            .WithDescription("Retrieves the ATP policy for Office 365")
            .RequirePermission("Exchange.Atp.Read", "Get ATP policy");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        Guid tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetAtpPolicyQuery(tenantId);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<AtpPolicyDto>.SuccessResult(result, "Get ATP policy successful"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error with get atp policy"
            );
        }
    }
}
