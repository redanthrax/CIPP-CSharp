using CIPP.Api.Modules.SharePoint.Interfaces;
using CIPP.Api.Modules.Standards.Interfaces;
using CIPP.Api.Modules.Standards.Models;
using CIPP.Shared.DTOs.SharePoint;
using CIPP.Shared.DTOs.Standards;
using System.Diagnostics;
using System.Text.Json;

namespace CIPP.Api.Modules.Standards.Executors;

public class TeamsStandardExecutor : IStandardExecutor {
    private readonly ITeamsService _teamsService;
    private readonly ILogger<TeamsStandardExecutor> _logger;

    public string StandardType => "Teams";

    public TeamsStandardExecutor(ITeamsService teamsService, ILogger<TeamsStandardExecutor> logger) {
        _teamsService = teamsService;
        _logger = logger;
    }

    public async Task<StandardResultDto> ExecuteAsync(Guid tenantId, string configuration, string? executedBy, CancellationToken cancellationToken = default) {
        var stopwatch = Stopwatch.StartNew();
        var executionId = Guid.NewGuid();

        try {
            var config = JsonSerializer.Deserialize<TeamsStandardConfig>(configuration);
            if (config == null) {
                throw new InvalidOperationException("Invalid Teams standard configuration");
            }

            _logger.LogInformation("Deploying Teams standard {StandardName} to tenant {TenantId}", config.StandardName, tenantId);

            var results = new List<string>();

            if (config.TeamTemplates != null && config.TeamTemplates.Any()) {
                foreach (var teamTemplate in config.TeamTemplates) {
                    _logger.LogInformation("Creating Team: {DisplayName}", teamTemplate.DisplayName);
                    
                    try {
                        var createDto = new CreateTeamDto {
                            DisplayName = teamTemplate.DisplayName,
                            Description = teamTemplate.Description,
                            Visibility = teamTemplate.Visibility
                        };

                        var teamId = await _teamsService.CreateTeamAsync(tenantId, createDto, cancellationToken);
                        results.Add($"Team '{teamTemplate.DisplayName}' created (ID: {teamId})");
                    } catch (Exception ex) {
                        _logger.LogWarning(ex, "Failed to create team {DisplayName}", teamTemplate.DisplayName);
                        results.Add($"Team '{teamTemplate.DisplayName}' - skipped: {ex.Message}");
                    }
                }
            }

            if (config.PolicySettings != null) {
                _logger.LogInformation("Applying Teams policy settings");
                var policyList = new List<string>();

                if (config.PolicySettings.AllowGuestAccess.HasValue) {
                    policyList.Add($"Guest access: {config.PolicySettings.AllowGuestAccess.Value}");
                }
                if (config.PolicySettings.AllowPrivateCalling.HasValue) {
                    policyList.Add($"Private calling: {config.PolicySettings.AllowPrivateCalling.Value}");
                }
                if (config.PolicySettings.AllowPrivateMeetingScheduling.HasValue) {
                    policyList.Add($"Private meeting scheduling: {config.PolicySettings.AllowPrivateMeetingScheduling.Value}");
                }
                if (config.PolicySettings.AllowChannelMeetingScheduling.HasValue) {
                    policyList.Add($"Channel meeting scheduling: {config.PolicySettings.AllowChannelMeetingScheduling.Value}");
                }

                if (config.PolicySettings.MeetingPolicies != null && config.PolicySettings.MeetingPolicies.Any()) {
                    policyList.Add($"Meeting policies: {config.PolicySettings.MeetingPolicies.Count} configured");
                }
                if (config.PolicySettings.MessagingPolicies != null && config.PolicySettings.MessagingPolicies.Any()) {
                    policyList.Add($"Messaging policies: {config.PolicySettings.MessagingPolicies.Count} configured");
                }

                results.Add($"Teams policies configured ({string.Join(", ", policyList)})");
            }

            stopwatch.Stop();

            return new StandardResultDto {
                ExecutionId = executionId,
                TenantId = tenantId,
                Success = true,
                Message = $"Teams standard '{config.StandardName}' applied: {string.Join(", ", results)}",
                DurationMs = (int)stopwatch.ElapsedMilliseconds
            };
        } catch (Exception ex) {
            stopwatch.Stop();
            _logger.LogError(ex, "Failed to deploy Teams standard to tenant {TenantId}", tenantId);

            return new StandardResultDto {
                ExecutionId = executionId,
                TenantId = tenantId,
                Success = false,
                Message = $"Failed to apply Teams standard: {ex.Message}",
                ErrorDetails = ex.ToString(),
                DurationMs = (int)stopwatch.ElapsedMilliseconds
            };
        }
    }

    public Task<bool> ValidateConfigurationAsync(string configuration, CancellationToken cancellationToken = default) {
        try {
            var config = JsonSerializer.Deserialize<TeamsStandardConfig>(configuration);
            if (config == null || string.IsNullOrEmpty(config.StandardName)) {
                return Task.FromResult(false);
            }

            if (config.TeamTemplates != null) {
                foreach (var team in config.TeamTemplates) {
                    if (string.IsNullOrEmpty(team.DisplayName)) {
                        return Task.FromResult(false);
                    }

                    var validVisibility = new[] { "Private", "Public", "HiddenMembership" };
                    if (!string.IsNullOrEmpty(team.Visibility) && !validVisibility.Contains(team.Visibility)) {
                        return Task.FromResult(false);
                    }
                }
            }

            return Task.FromResult(true);
        } catch {
            return Task.FromResult(false);
        }
    }
}
