using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Shared.DTOs;

namespace CIPP.Api.Modules.MsGraph.Endpoints;

public static class TestGraph {
    public static void MapTestGraph(this RouteGroupBuilder group) {
        group.MapGet("/test", Handle)
            .WithName("TestGraph")
            .WithSummary("Test Microsoft Graph connectivity")
            .WithDescription("Tests the connection to Microsoft Graph and returns organization information")
            .RequireAuthorization("CombinedAuth");
    }

    private static async Task<IResult> Handle(
        IMicrosoftGraphService graphService,
        CancellationToken cancellationToken = default) {
        try {
            var organization = await graphService.GetOrganizationAsync();
            
            var response = new {
                Success = true,
                OrganizationName = organization?.DisplayName ?? "Unknown",
                Message = "Microsoft Graph connection successful"
            };

            return Results.Ok(Response<object>.SuccessResult(response, "Graph connection successful"));
        } catch (Exception ex) {
            return Results.Json(
                Response<object>.ErrorResult("Graph connection failed", new List<string> { ex.Message }),
                statusCode: 500
            );
        }
    }
}
