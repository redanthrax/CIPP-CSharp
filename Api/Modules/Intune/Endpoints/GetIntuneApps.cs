using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Intune.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Intune;
using DispatchR;

namespace CIPP.Api.Modules.Intune.Endpoints;

public static class GetIntuneApps {
    public static void MapGetIntuneApps(this RouteGroupBuilder group) {
        group.MapGet("/", Handle)
            .WithName("GetIntuneApps")
            .WithSummary("Get Intune applications")
            .WithDescription("Retrieves all mobile applications for the specified tenant")
            .RequirePermission("Endpoint.Application.Read", "View Intune applications");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetIntuneAppsQuery(tenantId);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<List<IntuneAppDto>>.SuccessResult(result, "Applications retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving Intune applications"
            );
        }
    }
}
