using CIPP.Api.Modules.ConditionalAccess.Interfaces;
using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.ConditionalAccess;
using Microsoft.Graph.Beta.Models;

namespace CIPP.Api.Modules.ConditionalAccess.Services;

public class ConditionalAccessPolicyService : IConditionalAccessPolicyService {
    private readonly IMicrosoftGraphService _graphService;
    private readonly ILogger<ConditionalAccessPolicyService> _logger;

    public ConditionalAccessPolicyService(IMicrosoftGraphService graphService, ILogger<ConditionalAccessPolicyService> logger) {
        _graphService = graphService;
        _logger = logger;
    }

    public async Task<PagedResponse<ConditionalAccessPolicyDto>> GetPoliciesAsync(Guid tenantId, PagingParameters? paging = null, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting conditional access policies for tenant {TenantId}", tenantId);
        
        paging ??= new PagingParameters();
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        var policies = await graphClient.Identity.ConditionalAccess.Policies.GetAsync(cancellationToken: cancellationToken);
        
        if (policies?.Value == null) {
            return new PagedResponse<ConditionalAccessPolicyDto> {
                Items = new List<ConditionalAccessPolicyDto>(),
                TotalCount = 0,
                PageNumber = paging.PageNumber,
                PageSize = paging.PageSize
            };
        }

        var allPolicies = policies.Value.Select(p => MapToPolicyDto(p, tenantId)).ToList();
        var pagedPolicies = allPolicies.Skip(paging.Skip).Take(paging.Take).ToList();
        
        return new PagedResponse<ConditionalAccessPolicyDto> {
            Items = pagedPolicies,
            TotalCount = allPolicies.Count,
            PageNumber = paging.PageNumber,
            PageSize = paging.PageSize
        };
    }

    public async Task<ConditionalAccessPolicyDto?> GetPolicyAsync(Guid tenantId, string policyId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting conditional access policy {PolicyId} for tenant {TenantId}", policyId, tenantId);
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        var policy = await graphClient.Identity.ConditionalAccess.Policies[policyId].GetAsync(cancellationToken: cancellationToken);
        
        return policy != null ? MapToPolicyDto(policy, tenantId) : null;
    }

    public async Task<ConditionalAccessPolicyDto> CreatePolicyAsync(CreateConditionalAccessPolicyDto createDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Creating conditional access policy {DisplayName} for tenant {TenantId}", createDto.DisplayName, createDto.TenantId);
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(createDto.TenantId);
        var policy = MapToGraphPolicy(createDto);
        
        var createdPolicy = await graphClient.Identity.ConditionalAccess.Policies.PostAsync(policy, cancellationToken: cancellationToken);
        
        if (createdPolicy == null) {
            throw new InvalidOperationException("Failed to create conditional access policy");
        }
        
        return MapToPolicyDto(createdPolicy, createDto.TenantId);
    }

    public async Task<ConditionalAccessPolicyDto> UpdatePolicyAsync(Guid tenantId, string policyId, UpdateConditionalAccessPolicyDto updateDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating conditional access policy {PolicyId} for tenant {TenantId}", policyId, tenantId);
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        var policy = MapToGraphPolicyUpdate(updateDto);
        
        var updatedPolicy = await graphClient.Identity.ConditionalAccess.Policies[policyId].PatchAsync(policy, cancellationToken: cancellationToken);
        
        if (updatedPolicy == null) {
            throw new InvalidOperationException("Failed to update conditional access policy");
        }
        
        return MapToPolicyDto(updatedPolicy, tenantId);
    }

    public async Task DeletePolicyAsync(Guid tenantId, string policyId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Deleting conditional access policy {PolicyId} for tenant {TenantId}", policyId, tenantId);
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        await graphClient.Identity.ConditionalAccess.Policies[policyId].DeleteAsync(cancellationToken: cancellationToken);
    }

    private static ConditionalAccessPolicyDto MapToPolicyDto(ConditionalAccessPolicy policy, Guid tenantId) {
        return new ConditionalAccessPolicyDto {
            Id = policy.Id ?? string.Empty,
            DisplayName = policy.DisplayName ?? string.Empty,
            Description = policy.Description,
            State = policy.State?.ToString() ?? string.Empty,
            CreatedDateTime = policy.CreatedDateTime?.DateTime,
            ModifiedDateTime = policy.ModifiedDateTime?.DateTime,
            TenantId = tenantId,
            Conditions = MapConditionsToDto(policy.Conditions),
            GrantControls = MapGrantControlsToDto(policy.GrantControls),
            SessionControls = MapSessionControlsToDto(policy.SessionControls)
        };
    }

    private static ConditionalAccessConditionsDto? MapConditionsToDto(ConditionalAccessConditionSet? conditions) {
        if (conditions == null) return null;

        return new ConditionalAccessConditionsDto {
            Users = conditions.Users != null ? new ConditionalAccessUserConditionDto {
                IncludeUsers = conditions.Users.IncludeUsers?.ToList(),
                ExcludeUsers = conditions.Users.ExcludeUsers?.ToList(),
                IncludeGroups = conditions.Users.IncludeGroups?.ToList(),
                ExcludeGroups = conditions.Users.ExcludeGroups?.ToList(),
                IncludeRoles = conditions.Users.IncludeRoles?.ToList(),
                ExcludeRoles = conditions.Users.ExcludeRoles?.ToList()
            } : null,
            Applications = conditions.Applications != null ? new ConditionalAccessApplicationConditionDto {
                IncludeApplications = conditions.Applications.IncludeApplications?.ToList(),
                ExcludeApplications = conditions.Applications.ExcludeApplications?.ToList(),
                IncludeUserActions = conditions.Applications.IncludeUserActions?.ToList()
            } : null,
            ClientAppTypes = conditions.ClientAppTypes?.Select(c => c.ToString() ?? string.Empty).ToList(),
            Locations = conditions.Locations != null ? new ConditionalAccessLocationConditionDto {
                IncludeLocations = conditions.Locations.IncludeLocations?.ToList(),
                ExcludeLocations = conditions.Locations.ExcludeLocations?.ToList()
            } : null,
            Platforms = conditions.Platforms != null ? new ConditionalAccessPlatformConditionDto {
                IncludePlatforms = conditions.Platforms.IncludePlatforms?.Select(p => p.ToString() ?? string.Empty).ToList(),
                ExcludePlatforms = conditions.Platforms.ExcludePlatforms?.Select(p => p.ToString() ?? string.Empty).ToList()
            } : null,
            Devices = conditions.Devices != null ? new ConditionalAccessDeviceConditionDto {
                IncludeDeviceStates = conditions.Devices.IncludeDeviceStates?.ToList(),
                ExcludeDeviceStates = conditions.Devices.ExcludeDeviceStates?.ToList()
            } : null,
            SignInRiskLevels = conditions.SignInRiskLevels?.Select(r => r.ToString() ?? string.Empty).ToList(),
            UserRiskLevels = conditions.UserRiskLevels?.Select(r => r.ToString() ?? string.Empty).ToList()
        };
    }

    private static ConditionalAccessGrantControlsDto? MapGrantControlsToDto(ConditionalAccessGrantControls? controls) {
        if (controls == null) return null;

        return new ConditionalAccessGrantControlsDto {
            Operator = controls.Operator ?? string.Empty,
            BuiltInControls = controls.BuiltInControls?.Select(c => c.ToString() ?? string.Empty).ToList(),
            CustomAuthenticationFactors = controls.CustomAuthenticationFactors?.ToList(),
            TermsOfUse = controls.TermsOfUse?.ToList()
        };
    }

    private static ConditionalAccessSessionControlsDto? MapSessionControlsToDto(ConditionalAccessSessionControls? controls) {
        if (controls == null) return null;

        return new ConditionalAccessSessionControlsDto {
            DisableResilienceDefaults = controls.DisableResilienceDefaults,
            ApplicationEnforcedRestrictionsIsEnabled = controls.ApplicationEnforcedRestrictions?.IsEnabled,
            CloudAppSecurityIsEnabled = controls.CloudAppSecurity?.IsEnabled,
            CloudAppSecurityType = controls.CloudAppSecurity?.CloudAppSecurityType?.ToString(),
            SignInFrequencyIsEnabled = controls.SignInFrequency?.IsEnabled,
            SignInFrequencyValue = controls.SignInFrequency?.Value,
            SignInFrequencyType = controls.SignInFrequency?.Type?.ToString(),
            PersistentBrowserIsEnabled = controls.PersistentBrowser?.IsEnabled,
            PersistentBrowserMode = controls.PersistentBrowser?.Mode?.ToString()
        };
    }

    private static ConditionalAccessPolicy MapToGraphPolicy(CreateConditionalAccessPolicyDto dto) {
        var policy = new ConditionalAccessPolicy {
            DisplayName = dto.DisplayName,
            Description = dto.Description,
            State = Enum.Parse<ConditionalAccessPolicyState>(dto.State, true)
        };

        if (dto.Conditions != null) {
            policy.Conditions = MapConditionsToGraph(dto.Conditions);
        }

        if (dto.GrantControls != null) {
            policy.GrantControls = MapGrantControlsToGraph(dto.GrantControls);
        }

        if (dto.SessionControls != null) {
            policy.SessionControls = MapSessionControlsToGraph(dto.SessionControls);
        }

        return policy;
    }

    private static ConditionalAccessPolicy MapToGraphPolicyUpdate(UpdateConditionalAccessPolicyDto dto) {
        var policy = new ConditionalAccessPolicy();

        if (dto.DisplayName != null) {
            policy.DisplayName = dto.DisplayName;
        }

        if (dto.Description != null) {
            policy.Description = dto.Description;
        }

        if (dto.State != null) {
            policy.State = Enum.Parse<ConditionalAccessPolicyState>(dto.State, true);
        }

        if (dto.Conditions != null) {
            policy.Conditions = MapConditionsToGraph(dto.Conditions);
        }

        if (dto.GrantControls != null) {
            policy.GrantControls = MapGrantControlsToGraph(dto.GrantControls);
        }

        if (dto.SessionControls != null) {
            policy.SessionControls = MapSessionControlsToGraph(dto.SessionControls);
        }

        return policy;
    }

    private static ConditionalAccessConditionSet MapConditionsToGraph(ConditionalAccessConditionsDto dto) {
        var conditions = new ConditionalAccessConditionSet();

        if (dto.Users != null) {
            conditions.Users = new ConditionalAccessUsers {
                IncludeUsers = dto.Users.IncludeUsers,
                ExcludeUsers = dto.Users.ExcludeUsers,
                IncludeGroups = dto.Users.IncludeGroups,
                ExcludeGroups = dto.Users.ExcludeGroups,
                IncludeRoles = dto.Users.IncludeRoles,
                ExcludeRoles = dto.Users.ExcludeRoles
            };
        }

        if (dto.Applications != null) {
            conditions.Applications = new ConditionalAccessApplications {
                IncludeApplications = dto.Applications.IncludeApplications,
                ExcludeApplications = dto.Applications.ExcludeApplications,
                IncludeUserActions = dto.Applications.IncludeUserActions
            };
        }

        if (dto.ClientAppTypes != null) {
            conditions.ClientAppTypes = dto.ClientAppTypes
                .Select(c => Enum.Parse<ConditionalAccessClientApp>(c, true))
                .Cast<ConditionalAccessClientApp?>()
                .ToList();
        }

        if (dto.Locations != null) {
            conditions.Locations = new ConditionalAccessLocations {
                IncludeLocations = dto.Locations.IncludeLocations,
                ExcludeLocations = dto.Locations.ExcludeLocations
            };
        }

        if (dto.Platforms != null) {
            conditions.Platforms = new ConditionalAccessPlatforms {
                IncludePlatforms = dto.Platforms.IncludePlatforms?
                    .Select(p => Enum.Parse<ConditionalAccessDevicePlatform>(p, true))
                    .Cast<ConditionalAccessDevicePlatform?>()
                    .ToList(),
                ExcludePlatforms = dto.Platforms.ExcludePlatforms?
                    .Select(p => Enum.Parse<ConditionalAccessDevicePlatform>(p, true))
                    .Cast<ConditionalAccessDevicePlatform?>()
                    .ToList()
            };
        }

        if (dto.SignInRiskLevels != null) {
            conditions.SignInRiskLevels = dto.SignInRiskLevels
                .Select(r => Enum.Parse<RiskLevel>(r, true))
                .Cast<RiskLevel?>()
                .ToList();
        }

        if (dto.UserRiskLevels != null) {
            conditions.UserRiskLevels = dto.UserRiskLevels
                .Select(r => Enum.Parse<RiskLevel>(r, true))
                .Cast<RiskLevel?>()
                .ToList();
        }

        return conditions;
    }

    private static ConditionalAccessGrantControls MapGrantControlsToGraph(ConditionalAccessGrantControlsDto dto) {
        return new ConditionalAccessGrantControls {
            Operator = dto.Operator,
            BuiltInControls = dto.BuiltInControls?
                .Select(c => Enum.Parse<ConditionalAccessGrantControl>(c, true))
                .Cast<ConditionalAccessGrantControl?>()
                .ToList(),
            CustomAuthenticationFactors = dto.CustomAuthenticationFactors,
            TermsOfUse = dto.TermsOfUse
        };
    }

    private static ConditionalAccessSessionControls MapSessionControlsToGraph(ConditionalAccessSessionControlsDto dto) {
        var controls = new ConditionalAccessSessionControls {
            DisableResilienceDefaults = dto.DisableResilienceDefaults
        };

        if (dto.ApplicationEnforcedRestrictionsIsEnabled.HasValue) {
            controls.ApplicationEnforcedRestrictions = new ApplicationEnforcedRestrictionsSessionControl {
                IsEnabled = dto.ApplicationEnforcedRestrictionsIsEnabled.Value
            };
        }

        if (dto.CloudAppSecurityIsEnabled.HasValue) {
            controls.CloudAppSecurity = new CloudAppSecuritySessionControl {
                IsEnabled = dto.CloudAppSecurityIsEnabled.Value,
                CloudAppSecurityType = dto.CloudAppSecurityType != null 
                    ? Enum.Parse<CloudAppSecuritySessionControlType>(dto.CloudAppSecurityType, true) 
                    : null
            };
        }

        if (dto.SignInFrequencyIsEnabled.HasValue) {
            controls.SignInFrequency = new SignInFrequencySessionControl {
                IsEnabled = dto.SignInFrequencyIsEnabled.Value,
                Value = dto.SignInFrequencyValue,
                Type = dto.SignInFrequencyType != null
                    ? Enum.Parse<SigninFrequencyType>(dto.SignInFrequencyType, true)
                    : null
            };
        }

        if (dto.PersistentBrowserIsEnabled.HasValue) {
            controls.PersistentBrowser = new PersistentBrowserSessionControl {
                IsEnabled = dto.PersistentBrowserIsEnabled.Value,
                Mode = dto.PersistentBrowserMode != null
                    ? Enum.Parse<PersistentBrowserSessionMode>(dto.PersistentBrowserMode, true)
                    : null
            };
        }

        return controls;
    }
}
