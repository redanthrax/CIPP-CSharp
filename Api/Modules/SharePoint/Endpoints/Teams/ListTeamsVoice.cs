using CIPP.Api.Extensions;
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
        HttpContext context,
        Guid tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var pagingParams = context.GetPagingParameters();
            var query = new GetTeamsVoiceQuery(tenantId, pagingParams);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<PagedResponse<TeamsVoiceDto>>.SuccessResult(result, "Teams voice numbers retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving Teams voice numbers"
            );
        }
    }
}
