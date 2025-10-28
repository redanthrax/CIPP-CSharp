using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Queries.OwaMailboxPolicies;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Handlers.OwaMailboxPolicies;

public class GetOwaMailboxPoliciesQueryHandler : IRequestHandler<GetOwaMailboxPoliciesQuery, Task<PagedResponse<OwaMailboxPolicyDto>>> {
    private readonly IOwaMailboxPolicyService _owaMailboxPolicyService;
    private readonly ILogger<GetOwaMailboxPoliciesQueryHandler> _logger;

    public GetOwaMailboxPoliciesQueryHandler(
        IOwaMailboxPolicyService owaMailboxPolicyService,
        ILogger<GetOwaMailboxPoliciesQueryHandler> logger) {
        _owaMailboxPolicyService = owaMailboxPolicyService;
        _logger = logger;
    }

    public async Task<PagedResponse<OwaMailboxPolicyDto>> Handle(GetOwaMailboxPoliciesQuery request, CancellationToken cancellationToken) {
        _logger.LogInformation("Handling GetOwaMailboxPoliciesQuery for tenant {TenantId}", request.TenantId);
        
        var policies = await _owaMailboxPolicyService.GetOwaMailboxPoliciesAsync(
            request.TenantId,
            request.PagingParams,
            cancellationToken);
        
        _logger.LogInformation("Retrieved {Count} OWA mailbox policies for tenant {TenantId}", 
            policies.TotalCount, request.TenantId);
        
        return policies;
    }
}
