using Microsoft.Graph;
using Microsoft.Graph.Beta.Models;
using BetaGraphServiceClient = Microsoft.Graph.Beta.GraphServiceClient;
using StableGraphServiceClient = Microsoft.Graph.GraphServiceClient;
using StableUnifiedRoleDefinition = Microsoft.Graph.Models.UnifiedRoleDefinition;
using StableUnifiedRoleAssignment = Microsoft.Graph.Models.UnifiedRoleAssignment;

namespace CIPP.Api.Modules.MsGraph.Services;

public class GraphRoleService {
    private readonly MicrosoftGraphService _graphService;
    private readonly CachedGraphRequestHandler _cacheHandler;
    private readonly GraphExceptionHandler _exceptionHandler;

    public GraphRoleService(MicrosoftGraphService graphService, CachedGraphRequestHandler cacheHandler, GraphExceptionHandler exceptionHandler) {
        _graphService = graphService;
        _cacheHandler = cacheHandler;
        _exceptionHandler = exceptionHandler;
    }

    public async Task<UnifiedRoleDefinitionCollectionResponse?> GetRoleDefinitionsAsync(Guid? tenantId = null) {
        return await _exceptionHandler.HandleAsync(async () => {
            var betaClient = await _graphService.GetGraphServiceClientAsync(tenantId);
            var stableClient = GetStableGraphClient(betaClient);
            
            var stableResult = await _cacheHandler.ExecuteAsync(
                tenantId,
                () => stableClient.RoleManagement.Directory.RoleDefinitions.GetAsync(requestConfiguration => { }),
                "directoryRoleManagement/directory/roleDefinitions",
                "GET"
            );
            
            return ConvertToBetaRoleDefinitionCollection(stableResult);
        }, tenantId, "getting role definitions");
    }

    public async Task<UnifiedRoleDefinition?> GetRoleDefinitionAsync(string roleId, Guid? tenantId = null) {
        return await _exceptionHandler.HandleAsync(async () => {
            var betaClient = await _graphService.GetGraphServiceClientAsync(tenantId);
            var stableClient = GetStableGraphClient(betaClient);
            
            var stableResult = await _cacheHandler.ExecuteAsync(
                tenantId,
                () => stableClient.RoleManagement.Directory.RoleDefinitions[roleId].GetAsync(requestConfiguration => { }),
                $"directoryRoleManagement/directory/roleDefinitions/{roleId}",
                "GET"
            );
            
            return ConvertToBetaRoleDefinition(stableResult);
        }, tenantId, $"getting role definition {roleId}");
    }

    public async Task<UnifiedRoleAssignmentCollectionResponse?> GetRoleAssignmentsAsync(Guid? tenantId = null, string? filter = null, string[]? expand = null) {
        return await _exceptionHandler.HandleAsync(async () => {
            var betaClient = await _graphService.GetGraphServiceClientAsync(tenantId);
            var stableClient = GetStableGraphClient(betaClient);
            
            var stableResult = await _cacheHandler.ExecuteAsync(
                tenantId,
                () => stableClient.RoleManagement.Directory.RoleAssignments.GetAsync(requestConfiguration => {
                    if (!string.IsNullOrEmpty(filter))
                        requestConfiguration.QueryParameters.Filter = filter;
                    if (expand != null && expand.Length > 0)
                        requestConfiguration.QueryParameters.Expand = expand;
                }),
                "directoryRoleManagement/directory/roleAssignments",
                "GET",
                new { filter, expand = expand != null ? string.Join(",", expand) : null }
            );
            
            return ConvertToBetaRoleAssignmentCollection(stableResult);
        }, tenantId, "getting role assignments");
    }

    public async Task<UnifiedRoleAssignment?> CreateRoleAssignmentAsync(UnifiedRoleAssignment roleAssignment, Guid? tenantId = null) {
        return await _exceptionHandler.HandleAsync(async () => {
            var betaClient = await _graphService.GetGraphServiceClientAsync(tenantId);
            var stableClient = GetStableGraphClient(betaClient);
            
            var stableAssignment = ConvertToStableRoleAssignment(roleAssignment);
            var stableResult = await _cacheHandler.ExecuteAsync(
                tenantId,
                () => stableClient.RoleManagement.Directory.RoleAssignments.PostAsync(stableAssignment, requestConfiguration => { }),
                "directoryRoleManagement/directory/roleAssignments",
                "POST"
            );
            
            return ConvertToBetaRoleAssignment(stableResult);
        }, tenantId, "creating role assignment");
    }

    public async Task DeleteRoleAssignmentAsync(string assignmentId, Guid? tenantId = null) {
        await _exceptionHandler.HandleAsync(async () => {
            var betaClient = await _graphService.GetGraphServiceClientAsync(tenantId);
            var stableClient = GetStableGraphClient(betaClient);
            await _cacheHandler.ExecuteAsync(
                tenantId,
                () => stableClient.RoleManagement.Directory.RoleAssignments[assignmentId].DeleteAsync(requestConfiguration => { }),
                $"directoryRoleManagement/directory/roleAssignments/{assignmentId}",
                "DELETE"
            );
            return Task.CompletedTask;
        }, tenantId, $"deleting role assignment {assignmentId}");
    }

    private StableGraphServiceClient GetStableGraphClient(BetaGraphServiceClient betaClient) {
        return new StableGraphServiceClient(betaClient.RequestAdapter);
    }

    private UnifiedRoleDefinitionCollectionResponse? ConvertToBetaRoleDefinitionCollection(
        Microsoft.Graph.Models.UnifiedRoleDefinitionCollectionResponse? stableResponse) {
        if (stableResponse == null) return null;
        
        return new UnifiedRoleDefinitionCollectionResponse {
            OdataCount = stableResponse.OdataCount,
            OdataNextLink = stableResponse.OdataNextLink,
            Value = stableResponse.Value?.Select(ConvertToBetaRoleDefinition).ToList()
        };
    }

    private UnifiedRoleDefinition? ConvertToBetaRoleDefinition(StableUnifiedRoleDefinition? stable) {
        if (stable == null) return null;
        
        return new UnifiedRoleDefinition {
            Id = stable.Id,
            DisplayName = stable.DisplayName,
            Description = stable.Description,
            TemplateId = stable.TemplateId,
            IsBuiltIn = stable.IsBuiltIn,
            IsEnabled = stable.IsEnabled,
            ResourceScopes = stable.ResourceScopes,
            RolePermissions = stable.RolePermissions?.Select(p => new UnifiedRolePermission {
                AllowedResourceActions = p.AllowedResourceActions,
                Condition = p.Condition
            }).ToList(),
            Version = stable.Version,
            InheritsPermissionsFrom = stable.InheritsPermissionsFrom?.Select(ConvertToBetaRoleDefinition).ToList()
        };
    }

    private UnifiedRoleAssignmentCollectionResponse? ConvertToBetaRoleAssignmentCollection(
        Microsoft.Graph.Models.UnifiedRoleAssignmentCollectionResponse? stableResponse) {
        if (stableResponse == null) return null;
        
        return new UnifiedRoleAssignmentCollectionResponse {
            OdataCount = stableResponse.OdataCount,
            OdataNextLink = stableResponse.OdataNextLink,
            Value = stableResponse.Value?.Select(ConvertToBetaRoleAssignment).ToList()
        };
    }

    private UnifiedRoleAssignment? ConvertToBetaRoleAssignment(StableUnifiedRoleAssignment? stable) {
        if (stable == null) return null;
        
        return new UnifiedRoleAssignment {
            Id = stable.Id,
            PrincipalId = stable.PrincipalId,
            RoleDefinitionId = stable.RoleDefinitionId,
            DirectoryScopeId = stable.DirectoryScopeId,
            AppScopeId = stable.AppScopeId,
            Condition = stable.Condition,
            Principal = stable.Principal != null ? new DirectoryObject {
                Id = stable.Principal.Id,
                DeletedDateTime = stable.Principal.DeletedDateTime
            } : null,
            RoleDefinition = ConvertToBetaRoleDefinition(stable.RoleDefinition)
        };
    }

    private StableUnifiedRoleAssignment ConvertToStableRoleAssignment(UnifiedRoleAssignment beta) {
        return new StableUnifiedRoleAssignment {
            Id = beta.Id,
            PrincipalId = beta.PrincipalId,
            RoleDefinitionId = beta.RoleDefinitionId,
            DirectoryScopeId = beta.DirectoryScopeId,
            AppScopeId = beta.AppScopeId,
            Condition = beta.Condition
        };
    }
}
