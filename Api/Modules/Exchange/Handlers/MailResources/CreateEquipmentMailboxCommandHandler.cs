using CIPP.Api.Modules.Exchange.Commands.MailResources;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.MailResources;

public class CreateEquipmentMailboxCommandHandler : IRequestHandler<CreateEquipmentMailboxCommand, Task<string>> {
    private readonly IMailResourceService _mailResourceService;

    public CreateEquipmentMailboxCommandHandler(IMailResourceService mailResourceService) {
        _mailResourceService = mailResourceService;
    }

    public async Task<string> Handle(CreateEquipmentMailboxCommand request, CancellationToken cancellationToken = default) {
        return await _mailResourceService.CreateEquipmentMailboxAsync(request.TenantId, request.CreateDto, cancellationToken);
    }
}
