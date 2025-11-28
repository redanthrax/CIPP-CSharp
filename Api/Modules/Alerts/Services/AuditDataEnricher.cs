using CIPP.Api.Modules.MsGraph.Interfaces;
using Microsoft.Graph.Beta;
using Microsoft.Graph.Beta.Models;
using System.Text.Json;

namespace CIPP.Api.Modules.Alerts.Services;

public class AuditDataEnricher {
    private readonly IMicrosoftGraphService _graphService;
    private readonly ILogger<AuditDataEnricher> _logger;

    public AuditDataEnricher(
        IMicrosoftGraphService graphService,
        ILogger<AuditDataEnricher> logger) {
        _graphService = graphService;
        _logger = logger;
    }

    public async Task<Dictionary<string, object>> EnrichAuditDataAsync(
        JsonElement auditData, 
        string tenantId) {
        var enrichedData = new Dictionary<string, object>();

        try {
            var guidPattern = @"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$";
            
            var users = await GetUsersAsync(tenantId);
            var groups = await GetGroupsAsync(tenantId);
            var servicePrincipals = await GetServicePrincipalsAsync(tenantId);

            foreach (var property in auditData.EnumerateObject()) {
                var value = GetPropertyValue(property.Value);
                enrichedData[property.Name] = value;

                if (value is string stringValue && 
                    System.Text.RegularExpressions.Regex.IsMatch(stringValue, guidPattern)) {
                    var mappedValue = MapGuidToDisplayName(
                        stringValue, 
                        users, 
                        groups, 
                        servicePrincipals);
                    
                    if (mappedValue != null) {
                        enrichedData[$"CIPP{property.Name}"] = mappedValue;
                    }
                }
            }

            return enrichedData;
        } catch (Exception ex) {
            _logger.LogError(ex, "Error enriching audit data for tenant {TenantId}", tenantId);
            return enrichedData;
        }
    }

    private async Task<List<User>> GetUsersAsync(string tenantId) {
        try {
            var graphClient = await _graphService.GetGraphServiceClientAsync(Guid.Parse(tenantId));
            var result = await graphClient.Users
                .GetAsync(config => {
                    config.QueryParameters.Select = new[] { "id", "displayName", "userPrincipalName" };
                    config.QueryParameters.Top = 999;
                });
            
            return result?.Value ?? new List<User>();
        } catch (Exception ex) {
            _logger.LogWarning(ex, "Failed to get users for tenant {TenantId}", tenantId);
            return new List<User>();
        }
    }

    private async Task<List<Group>> GetGroupsAsync(string tenantId) {
        try {
            var graphClient = await _graphService.GetGraphServiceClientAsync(Guid.Parse(tenantId));
            var result = await graphClient.Groups
                .GetAsync(config => {
                    config.QueryParameters.Select = new[] { "id", "displayName" };
                    config.QueryParameters.Top = 999;
                });
            
            return result?.Value ?? new List<Group>();
        } catch (Exception ex) {
            _logger.LogWarning(ex, "Failed to get groups for tenant {TenantId}", tenantId);
            return new List<Group>();
        }
    }

    private async Task<List<ServicePrincipal>> GetServicePrincipalsAsync(string tenantId) {
        try {
            var graphClient = await _graphService.GetGraphServiceClientAsync(Guid.Parse(tenantId));
            var result = await graphClient.ServicePrincipals
                .GetAsync(config => {
                    config.QueryParameters.Select = new[] { "id", "displayName", "appId" };
                    config.QueryParameters.Top = 999;
                });
            
            return result?.Value ?? new List<ServicePrincipal>();
        } catch (Exception ex) {
            _logger.LogWarning(ex, "Failed to get service principals for tenant {TenantId}", tenantId);
            return new List<ServicePrincipal>();
        }
    }

    private string? MapGuidToDisplayName(
        string guid, 
        List<User> users, 
        List<Group> groups, 
        List<ServicePrincipal> servicePrincipals) {
        var user = users.FirstOrDefault(u => u.Id == guid);
        if (user != null) {
            return user.UserPrincipalName ?? user.DisplayName;
        }

        var group = groups.FirstOrDefault(g => g.Id == guid);
        if (group != null) {
            return group.DisplayName;
        }

        var sp = servicePrincipals.FirstOrDefault(s => s.Id == guid || s.AppId == guid);
        if (sp != null) {
            return sp.DisplayName;
        }

        return null;
    }

    private static object GetPropertyValue(JsonElement element) {
        return element.ValueKind switch {
            JsonValueKind.String => element.GetString() ?? string.Empty,
            JsonValueKind.Number => element.TryGetInt64(out var longValue) ? longValue : element.GetDouble(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => string.Empty,
            JsonValueKind.Array => element.ToString(),
            JsonValueKind.Object => element.ToString(),
            _ => element.ToString()
        };
    }
}
