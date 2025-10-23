using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Applications.Queries;
using CIPP.Api.Extensions;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;
using DispatchR;

namespace CIPP.Api.Modules.Applications.Endpoints;

public static class GetApplications {
    public static void MapGetApplications(this RouteGroupBuilder group) {
        group.MapGet("", Handle)
            .WithName("GetApplications")
            .WithSummary("Get all applications")
            .WithDescription("Retrieves all application registrations for the specified tenant with pagination support")
            .RequirePermission("Applications.Application.Read", "View applications");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        string tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var pagingParams = context.GetPagingParameters();
            var query = new GetApplicationsQuery(tenantId, pagingParams);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<PagedResponse<ApplicationDto>>.SuccessResult(result, "Applications retrieved successfully"));
        } catch (Exception ex) {
            return Results.Json(
                Response<PagedResponse<ApplicationDto>>.ErrorResult("Error retrieving applications", new List<string> { ex.Message }),
                statusCode: 500
            );
        }
    }
}
