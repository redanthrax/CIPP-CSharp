using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.Contacts;

public static class UpdateContact {
    public static void MapUpdateContact(this RouteGroupBuilder group) {
        group.MapPatch("/{contactId}", Handle)
            .WithName("UpdateContact")
            .WithSummary("Update contact")
            .WithDescription("Updates a contact")
            .RequirePermission("Exchange.Contact.ReadWrite", "Update contacts");
    }

    private static async Task<IResult> Handle(
        Guid tenantId,
        string contactId,
        UpdateContactDto updateDto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new UpdateContactCommand(tenantId, contactId, updateDto);
            await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult("Contact updated successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error updating contact"
            );
        }
    }
}
