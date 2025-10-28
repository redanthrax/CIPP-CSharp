using CIPP.Api.Modules.Exchange.Commands.MailResources;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.MailResources;

public class CreateRoomMailboxCommandHandler : IRequestHandler<CreateRoomMailboxCommand, Task<string>> {
    private readonly IMailResourceService _mailResourceService;

    public CreateRoomMailboxCommandHandler(IMailResourceService mailResourceService) {
        _mailResourceService = mailResourceService;
    }

    public async Task<string> Handle(CreateRoomMailboxCommand request, CancellationToken cancellationToken = default) {
        return await _mailResourceService.CreateRoomMailboxAsync(request.TenantId, request.CreateDto, cancellationToken);
    }
}
