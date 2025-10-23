using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class DeleteContactCommandHandler : IRequestHandler<DeleteContactCommand, Task> {
    private readonly IContactService _contactService;

    public DeleteContactCommandHandler(IContactService contactService) {
        _contactService = contactService;
    }

    public async Task Handle(DeleteContactCommand command, CancellationToken cancellationToken) {
        await _contactService.DeleteContactAsync(command.TenantId, command.ContactId, cancellationToken);
    }
}
