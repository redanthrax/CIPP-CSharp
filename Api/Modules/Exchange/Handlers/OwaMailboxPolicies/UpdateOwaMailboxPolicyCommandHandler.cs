using CIPP.Api.Modules.Exchange.Commands.OwaMailboxPolicies;
using CIPP.Api.Modules.Exchange.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.OwaMailboxPolicies;

public class UpdateOwaMailboxPolicyCommandHandler : IRequestHandler<UpdateOwaMailboxPolicyCommand, Task> {
    private readonly IOwaMailboxPolicyService _owaMailboxPolicyService;
    private readonly ILogger<UpdateOwaMailboxPolicyCommandHandler> _logger;

    public UpdateOwaMailboxPolicyCommandHandler(
        IOwaMailboxPolicyService owaMailboxPolicyService,
        ILogger<UpdateOwaMailboxPolicyCommandHandler> logger) {
        _owaMailboxPolicyService = owaMailboxPolicyService;
        _logger = logger;
    }

    public async Task Handle(UpdateOwaMailboxPolicyCommand request, CancellationToken cancellationToken) {
        _logger.LogInformation("Handling UpdateOwaMailboxPolicyCommand for policy {PolicyId} in tenant {TenantId}", 
            request.PolicyId, request.TenantId);
        
        await _owaMailboxPolicyService.UpdateOwaMailboxPolicyAsync(
            request.TenantId,
            request.PolicyId,
            request.UpdateDto,
            cancellationToken);
        
        _logger.LogInformation("Successfully updated OWA mailbox policy {PolicyId}", request.PolicyId);
    }
}
