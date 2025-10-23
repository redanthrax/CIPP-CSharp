using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Shared.DTOs;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.Contacts;

public static class DeleteContact {
    public static void MapDeleteContact(this RouteGroupBuilder group) {
        group.MapDelete("/{contactId}", Handle)
            .WithName("DeleteContact")
            .WithSummary("Delete contact")
            .WithDescription("Deletes a contact")
            .RequirePermission("Exchange.Contact.ReadWrite", "Delete contacts");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        string contactId,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new DeleteContactCommand(tenantId, contactId);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult("Contact deleted successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error deleting contact"
            );
        }
    }
}
