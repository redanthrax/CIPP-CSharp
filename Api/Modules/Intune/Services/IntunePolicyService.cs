using CIPP.Api.Modules.Intune.Interfaces;
using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Shared.DTOs.Intune;
using Microsoft.Graph.Beta;
using Microsoft.Graph.Beta.Models;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Text.Json;

namespace CIPP.Api.Modules.Intune.Services;

public class IntunePolicyService : IIntunePolicyService {
    private readonly IMicrosoftGraphService _graphService;
    private readonly ILogger<IntunePolicyService> _logger;

    public IntunePolicyService(IMicrosoftGraphService graphService, ILogger<IntunePolicyService> logger) {
        _graphService = graphService;
        _logger = logger;
    }

    public async Task<List<IntunePolicyDto>> GetPoliciesAsync(string tenantId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting Intune policies for tenant {TenantId}", tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        var policies = new List<IntunePolicyDto>();

        var deviceConfigs = await graphClient.DeviceManagement.DeviceConfigurations.GetAsync(config => {
            config.QueryParameters.Select = new[] { "id", "displayName", "lastModifiedDateTime", "roleScopeTagIds", "description" };
            config.QueryParameters.Expand = new[] { "assignments" };
            config.QueryParameters.Top = 1000;
        }, cancellationToken);

        if (deviceConfigs?.Value != null) {
            foreach (var config in deviceConfigs.Value) {
                policies.Add(MapToPolicyDto(config, tenantId, "DeviceConfigurations", DeterminePolicyType(config)));
            }
        }

        var groupPolicyConfigs = await graphClient.DeviceManagement.GroupPolicyConfigurations.GetAsync(config => {
            config.QueryParameters.Expand = new[] { "assignments" };
            config.QueryParameters.Top = 1000;
        }, cancellationToken);

        if (groupPolicyConfigs?.Value != null) {
            foreach (var config in groupPolicyConfigs.Value) {
                policies.Add(new IntunePolicyDto {
                    Id = config.Id ?? string.Empty,
                    DisplayName = config.DisplayName ?? string.Empty,
                    Description = config.Description,
                    LastModifiedDateTime = config.LastModifiedDateTime?.DateTime,
                    PolicyTypeName = "Administrative Templates",
                    URLName = "GroupPolicyConfigurations",
                    TenantId = tenantId
                });
            }
        }

        var configPolicies = await graphClient.DeviceManagement.ConfigurationPolicies.GetAsync(config => {
            config.QueryParameters.Expand = new[] { "assignments" };
            config.QueryParameters.Top = 1000;
        }, cancellationToken);

        if (configPolicies?.Value != null) {
            foreach (var config in configPolicies.Value) {
                policies.Add(new IntunePolicyDto {
                    Id = config.Id ?? string.Empty,
                    DisplayName = config.Name ?? string.Empty,
                    Description = config.Description,
                    LastModifiedDateTime = config.LastModifiedDateTime?.DateTime,
                    PolicyTypeName = "Device Configuration",
                    URLName = "ConfigurationPolicies",
                    TenantId = tenantId
                });
            }
        }

        return policies;
    }

    public async Task<IntunePolicyDto?> GetPolicyAsync(string tenantId, string policyId, string urlName, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting Intune policy {PolicyId} for tenant {TenantId}", policyId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);

        if (urlName == "DeviceConfigurations") {
            var config = await graphClient.DeviceManagement.DeviceConfigurations[policyId].GetAsync(cancellationToken: cancellationToken);
            return config != null ? MapToPolicyDto(config, tenantId, urlName, DeterminePolicyType(config)) : null;
        }

        return null;
    }

    public async Task<IntunePolicyDto> CreatePolicyAsync(string tenantId, CreateIntunePolicyDto policyDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Creating Intune policy {DisplayName} for tenant {TenantId}", policyDto.DisplayName, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        var policyJson = JsonDocument.Parse(policyDto.RawJson);
        var root = policyJson.RootElement;

        var policy = policyDto.TemplateType switch {
            "Device" => await CreateDeviceConfiguration(graphClient, policyDto, cancellationToken),
            "Catalog" => await CreateConfigurationPolicy(graphClient, policyDto, cancellationToken),
            "deviceCompliancePolicies" => await CreateCompliancePolicy(graphClient, policyDto, cancellationToken),
            _ => throw new NotSupportedException($"Policy template type {policyDto.TemplateType} is not yet supported")
        };

        return policy;
    }

    public async Task<IntunePolicyDto> UpdatePolicyAsync(string tenantId, string policyId, UpdateIntunePolicyDto policyDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating Intune policy {PolicyId} for tenant {TenantId}", policyId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);

        var policy = policyDto.TemplateType switch {
            "Device" => await UpdateDeviceConfiguration(graphClient, policyId, policyDto, cancellationToken),
            "Catalog" => await UpdateConfigurationPolicy(graphClient, policyId, policyDto, cancellationToken),
            "deviceCompliancePolicies" => await UpdateCompliancePolicy(graphClient, policyId, policyDto, cancellationToken),
            _ => throw new NotSupportedException($"Policy template type {policyDto.TemplateType} is not yet supported")
        };

        return policy;
    }

    public async Task DeletePolicyAsync(string tenantId, string policyId, string urlName, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Deleting Intune policy {PolicyId} for tenant {TenantId}", policyId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);

        if (urlName == "DeviceConfigurations") {
            await graphClient.DeviceManagement.DeviceConfigurations[policyId].DeleteAsync(cancellationToken: cancellationToken);
        } else if (urlName == "GroupPolicyConfigurations") {
            await graphClient.DeviceManagement.GroupPolicyConfigurations[policyId].DeleteAsync(cancellationToken: cancellationToken);
        } else if (urlName == "ConfigurationPolicies") {
            await graphClient.DeviceManagement.ConfigurationPolicies[policyId].DeleteAsync(cancellationToken: cancellationToken);
        }
    }

    private static IntunePolicyDto MapToPolicyDto(DeviceConfiguration config, string tenantId, string urlName, string policyType) {
        return new IntunePolicyDto {
            Id = config.Id ?? string.Empty,
            DisplayName = config.DisplayName ?? string.Empty,
            Description = config.Description,
            LastModifiedDateTime = config.LastModifiedDateTime?.DateTime,
            PolicyTypeName = policyType,
            URLName = urlName,
            RoleScopeTagIds = config.RoleScopeTagIds?.ToList(),
            TenantId = tenantId
        };
    }

    private async Task<IntunePolicyDto> CreateDeviceConfiguration(GraphServiceClient graphClient, CreateIntunePolicyDto policyDto, CancellationToken cancellationToken) {
        var config = JsonSerializer.Deserialize<DeviceConfiguration>(policyDto.RawJson) 
            ?? throw new InvalidOperationException("Failed to deserialize policy JSON");
        
        config.DisplayName = policyDto.DisplayName;
        config.Description = policyDto.Description;
        
        var created = await graphClient.DeviceManagement.DeviceConfigurations.PostAsync(config, cancellationToken: cancellationToken);
        
        return new IntunePolicyDto {
            Id = created?.Id ?? string.Empty,
            DisplayName = created?.DisplayName ?? policyDto.DisplayName,
            Description = created?.Description,
            LastModifiedDateTime = created?.LastModifiedDateTime?.DateTime,
            PolicyTypeName = "Device Configuration",
            URLName = "DeviceConfigurations",
            TenantId = string.Empty
        };
    }

    private async Task<IntunePolicyDto> CreateConfigurationPolicy(GraphServiceClient graphClient, CreateIntunePolicyDto policyDto, CancellationToken cancellationToken) {
        var config = JsonSerializer.Deserialize<DeviceManagementConfigurationPolicy>(policyDto.RawJson) 
            ?? throw new InvalidOperationException("Failed to deserialize policy JSON");
        
        config.Name = policyDto.DisplayName;
        config.Description = policyDto.Description;
        
        var created = await graphClient.DeviceManagement.ConfigurationPolicies.PostAsync(config, cancellationToken: cancellationToken);
        
        return new IntunePolicyDto {
            Id = created?.Id ?? string.Empty,
            DisplayName = created?.Name ?? policyDto.DisplayName,
            Description = created?.Description,
            LastModifiedDateTime = created?.LastModifiedDateTime?.DateTime,
            PolicyTypeName = "Settings Catalog",
            URLName = "ConfigurationPolicies",
            TenantId = string.Empty
        };
    }

    private async Task<IntunePolicyDto> CreateCompliancePolicy(GraphServiceClient graphClient, CreateIntunePolicyDto policyDto, CancellationToken cancellationToken) {
        var config = JsonSerializer.Deserialize<DeviceCompliancePolicy>(policyDto.RawJson) 
            ?? throw new InvalidOperationException("Failed to deserialize policy JSON");
        
        config.DisplayName = policyDto.DisplayName;
        config.Description = policyDto.Description;
        
        var created = await graphClient.DeviceManagement.DeviceCompliancePolicies.PostAsync(config, cancellationToken: cancellationToken);
        
        return new IntunePolicyDto {
            Id = created?.Id ?? string.Empty,
            DisplayName = created?.DisplayName ?? policyDto.DisplayName,
            Description = created?.Description,
            LastModifiedDateTime = created?.LastModifiedDateTime?.DateTime,
            PolicyTypeName = "Compliance Policy",
            URLName = "DeviceCompliancePolicies",
            TenantId = string.Empty
        };
    }

    private async Task<IntunePolicyDto> UpdateDeviceConfiguration(GraphServiceClient graphClient, string policyId, UpdateIntunePolicyDto policyDto, CancellationToken cancellationToken) {
        var config = JsonSerializer.Deserialize<DeviceConfiguration>(policyDto.RawJson) 
            ?? throw new InvalidOperationException("Failed to deserialize policy JSON");
        
        config.DisplayName = policyDto.DisplayName;
        config.Description = policyDto.Description;
        
        var updated = await graphClient.DeviceManagement.DeviceConfigurations[policyId].PatchAsync(config, cancellationToken: cancellationToken);
        
        return new IntunePolicyDto {
            Id = updated?.Id ?? policyId,
            DisplayName = updated?.DisplayName ?? policyDto.DisplayName,
            Description = updated?.Description,
            LastModifiedDateTime = updated?.LastModifiedDateTime?.DateTime,
            PolicyTypeName = "Device Configuration",
            URLName = "DeviceConfigurations",
            TenantId = string.Empty
        };
    }

    private async Task<IntunePolicyDto> UpdateConfigurationPolicy(GraphServiceClient graphClient, string policyId, UpdateIntunePolicyDto policyDto, CancellationToken cancellationToken) {
        var config = JsonSerializer.Deserialize<DeviceManagementConfigurationPolicy>(policyDto.RawJson) 
            ?? throw new InvalidOperationException("Failed to deserialize policy JSON");
        
        config.Name = policyDto.DisplayName;
        config.Description = policyDto.Description;
        
        var updated = await graphClient.DeviceManagement.ConfigurationPolicies[policyId].PatchAsync(config, cancellationToken: cancellationToken);
        
        return new IntunePolicyDto {
            Id = updated?.Id ?? policyId,
            DisplayName = updated?.Name ?? policyDto.DisplayName,
            Description = updated?.Description,
            LastModifiedDateTime = updated?.LastModifiedDateTime?.DateTime,
            PolicyTypeName = "Settings Catalog",
            URLName = "ConfigurationPolicies",
            TenantId = string.Empty
        };
    }

    private async Task<IntunePolicyDto> UpdateCompliancePolicy(GraphServiceClient graphClient, string policyId, UpdateIntunePolicyDto policyDto, CancellationToken cancellationToken) {
        var config = JsonSerializer.Deserialize<DeviceCompliancePolicy>(policyDto.RawJson) 
            ?? throw new InvalidOperationException("Failed to deserialize policy JSON");
        
        config.DisplayName = policyDto.DisplayName;
        config.Description = policyDto.Description;
        
        var updated = await graphClient.DeviceManagement.DeviceCompliancePolicies[policyId].PatchAsync(config, cancellationToken: cancellationToken);
        
        return new IntunePolicyDto {
            Id = updated?.Id ?? policyId,
            DisplayName = updated?.DisplayName ?? policyDto.DisplayName,
            Description = updated?.Description,
            LastModifiedDateTime = updated?.LastModifiedDateTime?.DateTime,
            PolicyTypeName = "Compliance Policy",
            URLName = "DeviceCompliancePolicies",
            TenantId = string.Empty
        };
    }

    private static string DeterminePolicyType(DeviceConfiguration config) {
        return config.OdataType switch {
            "#microsoft.graph.windowsIdentityProtectionConfiguration" => "Identity Protection",
            "#microsoft.graph.windows10EndpointProtectionConfiguration" => "Endpoint Protection",
            "#microsoft.graph.windows10CustomConfiguration" => "Custom",
            "#microsoft.graph.windows10DeviceFirmwareConfigurationInterface" => "Firmware Configuration",
            "#microsoft.graph.windowsDomainJoinConfiguration" => "Domain Join",
            "#microsoft.graph.windowsUpdateForBusinessConfiguration" => "Update Configuration",
            "#microsoft.graph.windowsHealthMonitoringConfiguration" => "Health Monitoring",
            "#microsoft.graph.macOSGeneralDeviceConfiguration" => "MacOS Configuration",
            "#microsoft.graph.macOSEndpointProtectionConfiguration" => "MacOS Endpoint Protection",
            "#microsoft.graph.androidWorkProfileGeneralDeviceConfiguration" => "Android Configuration",
            "#microsoft.graph.iosUpdateConfiguration" => "iOS Update Configuration",
            _ => "Device Configuration"
        };
    }
}
