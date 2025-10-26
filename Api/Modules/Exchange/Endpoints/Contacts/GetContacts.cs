using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.Contacts;

public static class GetContacts {
    public static void MapGetContacts(this RouteGroupBuilder group) {
        group.MapGet("/", Handle)
            .WithName("GetContacts")
            .WithSummary("Get contacts")
            .WithDescription("Retrieves all contacts for a tenant")
            .RequirePermission("Exchange.Contact.Read", "View contacts");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetContactsQuery(tenantId);
            var result = await mediator.Send(query, cancellationToken);

            return Results.Ok(Response<List<ContactDto>>.SuccessResult(result, "Contacts retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving contacts"
            );
        }
    }
}
