using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.Mailboxes;

public static class GetMailboxPermissions {
    public static void MapGetMailboxPermissions(this RouteGroupBuilder group) {
        group.MapGet("/{userId}/permissions", Handle)
            .WithName("GetMailboxPermissions")
            .WithSummary("Get mailbox permissions")
            .WithDescription("Retrieves permissions for a specific mailbox")
            .RequirePermission("Exchange.Mailbox.Read", "View mailbox permissions");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string userId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetMailboxPermissionsQuery(tenantId, userId);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<List<MailboxPermissionDto>>.SuccessResult(result, "Mailbox permissions retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving mailbox permissions"
            );
        }
    }
}
