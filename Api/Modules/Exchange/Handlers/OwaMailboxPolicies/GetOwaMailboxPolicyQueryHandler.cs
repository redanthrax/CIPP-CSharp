using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries.OwaMailboxPolicies;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.OwaMailboxPolicies;

public class GetOwaMailboxPolicyQueryHandler : IRequestHandler<GetOwaMailboxPolicyQuery, Task<OwaMailboxPolicyDto?>> {
    private readonly IOwaMailboxPolicyService _owaMailboxPolicyService;
    private readonly ILogger<GetOwaMailboxPolicyQueryHandler> _logger;

    public GetOwaMailboxPolicyQueryHandler(
        IOwaMailboxPolicyService owaMailboxPolicyService,
        ILogger<GetOwaMailboxPolicyQueryHandler> logger) {
        _owaMailboxPolicyService = owaMailboxPolicyService;
        _logger = logger;
    }

    public async Task<OwaMailboxPolicyDto?> Handle(GetOwaMailboxPolicyQuery request, CancellationToken cancellationToken) {
        _logger.LogInformation("Handling GetOwaMailboxPolicyQuery for policy {PolicyId} in tenant {TenantId}", 
            request.PolicyId, request.TenantId);
        
        var policy = await _owaMailboxPolicyService.GetOwaMailboxPolicyAsync(
            request.TenantId,
            request.PolicyId,
            cancellationToken);
        
        if (policy != null) {
            _logger.LogInformation("Retrieved OWA mailbox policy {PolicyId}", request.PolicyId);
        } else {
            _logger.LogWarning("OWA mailbox policy {PolicyId} not found", request.PolicyId);
        }
        
        return policy;
    }
}
