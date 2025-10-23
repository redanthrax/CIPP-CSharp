using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.Contacts;

public static class GetContact {
    public static void MapGetContact(this RouteGroupBuilder group) {
        group.MapGet("/{contactId}", Handle)
            .WithName("GetContact")
            .WithSummary("Get contact")
            .WithDescription("Retrieves details for a specific contact")
            .RequirePermission("Exchange.Contact.Read", "View contact details");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        string contactId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetContactQuery(tenantId, contactId);
            var result = await mediator.Send(query, cancellationToken);

            if (result == null) {
                return Results.NotFound(Response<ContactDetailsDto>.ErrorResult("Contact not found"));
            }

            return Results.Ok(Response<ContactDetailsDto>.SuccessResult(result, "Contact retrieved successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving contact"
            );
        }
    }
}
