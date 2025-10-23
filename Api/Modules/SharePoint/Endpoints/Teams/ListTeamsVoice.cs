using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.SharePoint.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.SharePoint;
using DispatchR;

namespace CIPP.Api.Modules.SharePoint.Endpoints.Teams;

public static class ListTeamsVoice {
    public static void MapListTeamsVoice(this RouteGroupBuilder group) {
        group.MapGet("/voice", Handle)
            .WithName("ListTeamsVoice")
            .WithSummary("List Teams voice phone numbers")
            .WithDescription("Retrieves Teams voice phone number assignments")
            .RequirePermission("Teams.Voice.Read", "View Teams voice");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetTeamsVoiceQuery(tenantId);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<List<TeamsVoiceDto>>.SuccessResult(result, "Teams voice numbers retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving Teams voice numbers"
            );
        }
    }
}
