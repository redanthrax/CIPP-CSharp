using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers;

public class UpdateSafeLinksPolicyCommandHandler : IRequestHandler<UpdateSafeLinksPolicyCommand, Task> {
    private readonly ISafePoliciesService _safePoliciesService;

    public UpdateSafeLinksPolicyCommandHandler(ISafePoliciesService safePoliciesService) {
        _safePoliciesService = safePoliciesService;
    }

    public async Task Handle(UpdateSafeLinksPolicyCommand command, CancellationToken cancellationToken) {
        await _safePoliciesService.UpdateSafeLinksPolicyAsync(command.TenantId, command.PolicyName, command.UpdateDto, cancellationToken);
    }
}
