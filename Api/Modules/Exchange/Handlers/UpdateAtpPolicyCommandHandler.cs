using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class UpdateAtpPolicyCommandHandler : IRequestHandler<UpdateAtpPolicyCommand, Task> {
    private readonly ISafePoliciesService _safePoliciesService;

    public UpdateAtpPolicyCommandHandler(ISafePoliciesService safePoliciesService) {
        _safePoliciesService = safePoliciesService;
    }

    public async Task Handle(UpdateAtpPolicyCommand command, CancellationToken cancellationToken) {
        await _safePoliciesService.UpdateAtpPolicyAsync(command.TenantId, command.UpdateDto, cancellationToken);
    }
}
