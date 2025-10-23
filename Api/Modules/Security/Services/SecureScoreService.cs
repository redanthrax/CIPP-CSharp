using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Api.Modules.Security.Interfaces;
using CIPP.Shared.DTOs.Security;
using Microsoft.Graph.Beta.Models;

namespace CIPP.Api.Modules.Security.Services;

public class SecureScoreService : ISecureScoreService {
    private readonly IMicrosoftGraphService _graphService;
    private readonly ILogger<SecureScoreService> _logger;

    public SecureScoreService(IMicrosoftGraphService graphService, ILogger<SecureScoreService> logger) {
        _graphService = graphService;
        _logger = logger;
    }

    public async Task<List<SecureScoreControlProfileDto>> GetControlProfilesAsync(string tenantId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting secure score control profiles for tenant {TenantId}", tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        var controlProfiles = await graphClient.Security.SecureScoreControlProfiles.GetAsync(cancellationToken: cancellationToken);

        if (controlProfiles?.Value == null) {
            return new List<SecureScoreControlProfileDto>();
        }

        return controlProfiles.Value.Select(MapToControlProfileDto).ToList();
    }

    public async Task<SecureScoreControlProfileDto?> GetControlProfileAsync(string tenantId, string controlName, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting secure score control profile {ControlName} for tenant {TenantId}", controlName, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        var controlProfile = await graphClient.Security.SecureScoreControlProfiles[controlName].GetAsync(cancellationToken: cancellationToken);

        return controlProfile != null ? MapToControlProfileDto(controlProfile) : null;
    }

    public async Task UpdateControlProfileAsync(string tenantId, string controlName, UpdateSecureScoreControlDto updateDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating secure score control profile {ControlName} for tenant {TenantId}", controlName, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);

        var controlProfile = new SecureScoreControlProfile {
            ControlStateUpdates = new List<SecureScoreControlStateUpdate> {
                new SecureScoreControlStateUpdate {
                    State = updateDto.State,
                    AssignedTo = null,
                    Comment = updateDto.Comment,
                    UpdatedBy = null,
                    UpdatedDateTime = DateTimeOffset.UtcNow
                }
            }
        };

        await graphClient.Security.SecureScoreControlProfiles[controlName].PatchAsync(controlProfile, cancellationToken: cancellationToken);
    }

    private static SecureScoreControlProfileDto MapToControlProfileDto(SecureScoreControlProfile profile) {
        return new SecureScoreControlProfileDto {
            Id = profile.Id ?? string.Empty,
            Title = profile.Title,
            State = profile.ControlStateUpdates?.OrderByDescending(u => u.UpdatedDateTime).FirstOrDefault()?.State,
            AzureTenantId = profile.AzureTenantId,
            ControlCategory = profile.ControlCategory,
            MaxScore = profile.MaxScore.HasValue ? (int)profile.MaxScore.Value : null,
            Rank = profile.Rank.HasValue ? profile.Rank.Value.ToString() : null,
            Remediation = profile.Remediation != null ? new List<string> { profile.Remediation } : null,
            RemediationImpact = profile.RemediationImpact,
            UserImpact = profile.UserImpact,
            ImplementationCost = profile.ImplementationCost,
            Deprecated = profile.Deprecated
        };
    }
}
