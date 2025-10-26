using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Applications.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;
using DispatchR;

namespace CIPP.Api.Modules.Applications.Endpoints;

public static class GetApplication {
    public static void MapGetApplication(this RouteGroupBuilder group) {
        group.MapGet("/{applicationId}", Handle)
            .WithName("GetApplication")
            .WithSummary("Get application by ID")
            .WithDescription("Retrieves a specific application by ID")
            .RequirePermission("Applications.Application.Read", "View applications");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string applicationId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetApplicationQuery(tenantId, applicationId);
            var result = await mediator.Send(query, cancellationToken);

            if (result == null) {
                return Results.NotFound(Response<ApplicationDto>.ErrorResult("Application not found", new List<string> { $"Application {applicationId} not found" }));
            }

            return Results.Ok(Response<ApplicationDto>.SuccessResult(result, "Application retrieved successfully"));
        } catch (Exception ex) {
            return Results.Json(
                Response<ApplicationDto>.ErrorResult("Error retrieving application", new List<string> { ex.Message }),
                statusCode: 500
            );
        }
    }
}
