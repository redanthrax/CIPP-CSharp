using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Api.Modules.Tenants.Queries;
using CIPP.Shared.DTOs.Tenants;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Tenants.Handlers;

public class GetOrganizationQueryHandler : IRequestHandler<GetOrganizationQuery, Task<OrganizationDto?>> {
    private readonly IMicrosoftGraphService _graphService;
    private readonly ILogger<GetOrganizationQueryHandler> _logger;

    public GetOrganizationQueryHandler(
        IMicrosoftGraphService graphService,
        ILogger<GetOrganizationQueryHandler> logger) {
        _graphService = graphService;
        _logger = logger;
    }

    public async Task<OrganizationDto?> Handle(GetOrganizationQuery request, CancellationToken cancellationToken) {
        try {
            _logger.LogInformation("Retrieving organization info for tenant {TenantId}", request.TenantId);

            var organization = await _graphService.GetOrganizationAsync(request.TenantId);
            if (organization == null) {
                return null;
            }

            var verifiedDomains = organization.VerifiedDomains?.Select(d => new VerifiedDomainDto {
                Name = d.Name ?? string.Empty,
                IsDefault = d.IsDefault ?? false
            }).ToList() ?? new List<VerifiedDomainDto>();

            var assignedPlans = organization.AssignedPlans?.Select(p => new AssignedPlanDto(
                p.AssignedDateTime?.ToString() ?? string.Empty,
                p.CapabilityStatus?.ToString() ?? string.Empty,
                p.Service ?? string.Empty,
                p.ServicePlanId?.ToString() ?? string.Empty
            )).ToList() ?? new List<AssignedPlanDto>();

            return new OrganizationDto {
                Id = organization.Id ?? string.Empty,
                DisplayName = organization.DisplayName ?? string.Empty,
                VerifiedDomains = verifiedDomains,
                OnPremisesSyncEnabled = organization.OnPremisesSyncEnabled,
                AssignedPlans = assignedPlans
            };
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to retrieve organization info for tenant {TenantId}", request.TenantId);
            return null;
        }
    }
}
