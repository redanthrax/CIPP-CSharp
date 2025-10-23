using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.Contacts;

public static class CreateContact {
    public static void MapCreateContact(this RouteGroupBuilder group) {
        group.MapPost("/", Handle)
            .WithName("CreateContact")
            .WithSummary("Create contact")
            .WithDescription("Creates a new contact")
            .RequirePermission("Exchange.Contact.ReadWrite", "Create contacts");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        CreateContactDto createDto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new CreateContactCommand(tenantId, createDto);
            var contactId = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<string>.SuccessResult(contactId, "Contact created successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error creating contact"
            );
        }
    }
}
