using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Intune.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Intune;
using DispatchR;

namespace CIPP.Api.Modules.Intune.Endpoints;

public static class GetIntunePolicies {
    public static void MapGetIntunePolicies(this RouteGroupBuilder group) {
        group.MapGet("/", Handle)
            .WithName("GetIntunePolicies")
            .WithSummary("Get all Intune policies")
            .WithDescription("Retrieves all device management policies for the specified tenant")
            .RequirePermission("Endpoint.MEM.Read", "View Intune policies");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetIntunePoliciesQuery(tenantId);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<List<IntunePolicyDto>>.SuccessResult(result, "Policies retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving Intune policies"
            );
        }
    }
}
