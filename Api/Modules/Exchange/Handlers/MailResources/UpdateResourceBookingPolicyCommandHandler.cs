using CIPP.Api.Modules.Exchange.Commands.MailResources;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.MailResources;

public class UpdateResourceBookingPolicyCommandHandler : IRequestHandler<UpdateResourceBookingPolicyCommand, Task> {
    private readonly IMailResourceService _mailResourceService;

    public UpdateResourceBookingPolicyCommandHandler(IMailResourceService mailResourceService) {
        _mailResourceService = mailResourceService;
    }

    public async Task Handle(UpdateResourceBookingPolicyCommand request, CancellationToken cancellationToken = default) {
        await _mailResourceService.UpdateResourceBookingPolicyAsync(request.TenantId, request.Identity, request.UpdateDto, cancellationToken);
    }
}
