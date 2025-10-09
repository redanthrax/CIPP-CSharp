using CIPP.Api.Data;
using CIPP.Api.Modules.Microsoft.Interfaces;
using CIPP.Api.Modules.Tenants.Models;
using CIPP.Api.Modules.Tenants.Queries;
using CIPP.Shared.DTOs.Tenants;
using DispatchR.Abstractions.Send;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Tenants.Handlers;

public class GetTenantDetailsQueryHandler : IRequestHandler<GetTenantDetailsQuery, Task<TenantDetailsDto>> {
    private readonly ApplicationDbContext _context;
    private readonly IMicrosoftGraphService _graphService;

    public GetTenantDetailsQueryHandler(ApplicationDbContext context, IMicrosoftGraphService graphService) {
        _context = context;
        _graphService = graphService;
    }

    public async Task<TenantDetailsDto> Handle(GetTenantDetailsQuery request, CancellationToken cancellationToken) {
        var tenant = await _context.GetEntitySet<Tenant>()
            .Include(t => t.Properties)
            .Include(t => t.Domains)
            .FirstOrDefaultAsync(t => t.Id == request.TenantId, cancellationToken);

        if (tenant == null) {
            throw new InvalidOperationException($"Tenant with ID {request.TenantId} not found");
        }

        var groupMemberships = await _context.Set<TenantGroupMembership>()
            .Include(m => m.TenantGroup)
            .Where(m => m.TenantId == tenant.Id)
            .ToListAsync(cancellationToken);

        var groups = groupMemberships.Select(m => new TenantGroupDto(
            m.TenantGroup.Id,
            m.TenantGroup.Name,
            m.TenantGroup.Description,
            m.TenantGroup.CreatedAt,
            m.TenantGroup.CreatedBy,
            m.TenantGroup.UpdatedAt,
            m.TenantGroup.UpdatedBy,
            null
        )).ToList();

        OrganizationDataDto? organizationData = null;
        try {
            var organization = await _graphService.GetOrganizationAsync(tenant.TenantId);
            if (organization != null) {
                var assignedPlans = organization.AssignedPlans?.Select(p => new AssignedPlanDto(
                    p.AssignedDateTime?.ToString() ?? string.Empty,
                    p.CapabilityStatus?.ToString() ?? string.Empty,
                    p.Service ?? string.Empty,
                    p.ServicePlanId?.ToString() ?? string.Empty
                )).ToList() ?? new List<AssignedPlanDto>();

                organizationData = new OrganizationDataDto(
                    organization.DisplayName ?? string.Empty,
                    organization.City ?? string.Empty,
                    organization.Country ?? string.Empty,
                    organization.CountryLetterCode ?? string.Empty,
                    organization.Street ?? string.Empty,
                    organization.State ?? string.Empty,
                    organization.PostalCode ?? string.Empty,
                    organization.BusinessPhones?.ToList() ?? new List<string>(),
                    organization.TechnicalNotificationMails?.ToList() ?? new List<string>(),
                    organization.TenantType ?? string.Empty,
                    organization.CreatedDateTime?.ToString() ?? string.Empty,
                    organization.OnPremisesLastPasswordSyncDateTime?.ToString() ?? string.Empty,
                    organization.OnPremisesLastSyncDateTime?.ToString() ?? string.Empty,
                    organization.OnPremisesSyncEnabled ?? false,
                    assignedPlans
                );
            }
        } catch (Exception) {
        }

        return new TenantDetailsDto(
            tenant.Id,
            tenant.TenantId,
            tenant.DisplayName,
            tenant.TenantAlias,
            tenant.DefaultDomainName,
            tenant.InitialDomainName,
            tenant.Status,
            tenant.CreatedAt,
            tenant.CreatedBy,
            tenant.Metadata,
            tenant.DomainList,
            tenant.GraphErrorCount,
            tenant.LastSyncAt,
            tenant.Excluded,
            tenant.ExcludeUser,
            tenant.ExcludeDate,
            tenant.DelegatedPrivilegeStatus,
            tenant.RequiresRefresh,
            tenant.LastGraphError,
            tenant.LastRefresh,
            groups,
            organizationData
        );
    }
}