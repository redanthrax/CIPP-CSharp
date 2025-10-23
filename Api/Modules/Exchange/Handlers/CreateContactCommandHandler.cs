using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, Task<string>> {
    private readonly IContactService _contactService;

    public CreateContactCommandHandler(IContactService contactService) {
        _contactService = contactService;
    }

    public async Task<string> Handle(CreateContactCommand command, CancellationToken cancellationToken) {
        return await _contactService.CreateContactAsync(command.TenantId, command.CreateDto, cancellationToken);
    }
}
