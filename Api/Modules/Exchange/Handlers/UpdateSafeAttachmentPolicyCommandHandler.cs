using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class UpdateSafeAttachmentPolicyCommandHandler : IRequestHandler<UpdateSafeAttachmentPolicyCommand, Task> {
    private readonly ISafePoliciesService _safePoliciesService;

    public UpdateSafeAttachmentPolicyCommandHandler(ISafePoliciesService safePoliciesService) {
        _safePoliciesService = safePoliciesService;
    }

    public async Task Handle(UpdateSafeAttachmentPolicyCommand command, CancellationToken cancellationToken) {
        await _safePoliciesService.UpdateSafeAttachmentPolicyAsync(command.TenantId, command.PolicyName, command.UpdateDto, cancellationToken);
    }
}
