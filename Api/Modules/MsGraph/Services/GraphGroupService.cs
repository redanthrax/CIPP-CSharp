using Microsoft.Graph.Beta;
using Microsoft.Graph.Beta.Models;
using CIPP.Shared.DTOs;

namespace CIPP.Api.Modules.MsGraph.Services;

public class GraphGroupService {
    private readonly MicrosoftGraphService _graphService;
    private readonly CachedGraphRequestHandler _cacheHandler;
    private readonly GraphExceptionHandler _exceptionHandler;

    public GraphGroupService(MicrosoftGraphService graphService, CachedGraphRequestHandler cacheHandler, GraphExceptionHandler exceptionHandler) {
        _graphService = graphService;
        _cacheHandler = cacheHandler;
        _exceptionHandler = exceptionHandler;
    }

    public async Task<GroupCollectionResponse?> ListGroupsAsync(Guid? tenantId = null, string? filter = null, string[]? select = null, PagingParameters? paging = null) {
        return await _exceptionHandler.HandleAsync(async () => {
            var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
            paging ??= new PagingParameters();
            
            return await _cacheHandler.ExecuteAsync(
                tenantId,
                () => graphClient.Groups.GetAsync(requestConfiguration => {
                    if (!string.IsNullOrEmpty(filter))
                        requestConfiguration.QueryParameters.Filter = filter;
                    if (select != null && select.Length > 0)
                        requestConfiguration.QueryParameters.Select = select;
                    requestConfiguration.QueryParameters.Top = paging.PageSize;
                }),
                "groups",
                "GET",
                new { filter, select = select != null ? string.Join(",", select) : null, pageNumber = paging.PageNumber, pageSize = paging.PageSize, skipToken = paging.SkipToken }
            );
        }, tenantId, "listing groups");
    }

    public async Task<Group?> GetGroupAsync(string groupId, Guid? tenantId = null) {
        return await _exceptionHandler.HandleAsync(async () => {
            var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
            return await _cacheHandler.ExecuteAsync(
                tenantId,
                () => graphClient.Groups[groupId].GetAsync(),
                $"groups/{groupId}",
                "GET"
            );
        }, tenantId, $"getting group {groupId}");
    }

    public async Task<Group?> CreateGroupAsync(Group group, Guid? tenantId = null) {
        return await _exceptionHandler.HandleAsync(async () => {
            var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
            return await _cacheHandler.ExecuteAsync(
                tenantId,
                () => graphClient.Groups.PostAsync(group),
                "groups",
                "POST"
            );
        }, tenantId, "creating group");
    }

    public async Task<Group?> UpdateGroupAsync(string groupId, Group group, Guid? tenantId = null) {
        return await _exceptionHandler.HandleAsync(async () => {
            var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
            return await _cacheHandler.ExecuteAsync(
                tenantId,
                () => graphClient.Groups[groupId].PatchAsync(group),
                $"groups/{groupId}",
                "PATCH"
            );
        }, tenantId, $"updating group {groupId}");
    }

    public async Task DeleteGroupAsync(string groupId, Guid? tenantId = null) {
        await _exceptionHandler.HandleAsync(async () => {
            var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
            await _cacheHandler.ExecuteAsync(
                tenantId,
                () => graphClient.Groups[groupId].DeleteAsync(),
                $"groups/{groupId}",
                "DELETE"
            );
            return Task.CompletedTask;
        }, tenantId, $"deleting group {groupId}");
    }

    public async Task<DirectoryObjectCollectionResponse?> GetGroupMembersAsync(string groupId, Guid? tenantId = null) {
        return await _exceptionHandler.HandleAsync(async () => {
            var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
            return await _cacheHandler.ExecuteAsync(
                tenantId,
                () => graphClient.Groups[groupId].Members.GetAsync(),
                $"groups/{groupId}/members",
                "GET"
            );
        }, tenantId, $"getting group members for {groupId}");
    }

    public async Task<DirectoryObjectCollectionResponse?> GetGroupOwnersAsync(string groupId, Guid? tenantId = null) {
        return await _exceptionHandler.HandleAsync(async () => {
            var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
            return await _cacheHandler.ExecuteAsync(
                tenantId,
                () => graphClient.Groups[groupId].Owners.GetAsync(),
                $"groups/{groupId}/owners",
                "GET"
            );
        }, tenantId, $"getting group owners for {groupId}");
    }

    public async Task AddGroupMemberAsync(string groupId, string userId, Guid? tenantId = null) {
        await _exceptionHandler.HandleAsync(async () => {
            var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
            var requestBody = new ReferenceCreate { OdataId = $"https://graph.microsoft.com/beta/users/{userId}" };
            await _cacheHandler.ExecuteAsync(
                tenantId,
                () => graphClient.Groups[groupId].Members.Ref.PostAsync(requestBody),
                $"groups/{groupId}/members",
                "POST"
            );
            return Task.CompletedTask;
        }, tenantId, $"adding member {userId} to group {groupId}");
    }

    public async Task AddGroupOwnerAsync(string groupId, string userId, Guid? tenantId = null) {
        await _exceptionHandler.HandleAsync(async () => {
            var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
            var requestBody = new ReferenceCreate { OdataId = $"https://graph.microsoft.com/beta/users/{userId}" };
            await _cacheHandler.ExecuteAsync(
                tenantId,
                () => graphClient.Groups[groupId].Owners.Ref.PostAsync(requestBody),
                $"groups/{groupId}/owners",
                "POST"
            );
            return Task.CompletedTask;
        }, tenantId, $"adding owner {userId} to group {groupId}");
    }

    public async Task RemoveGroupMemberAsync(string groupId, string userId, Guid? tenantId = null) {
        await _exceptionHandler.HandleAsync(async () => {
            var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
            await _cacheHandler.ExecuteAsync(
                tenantId,
                () => graphClient.Groups[groupId].Members[userId].Ref.DeleteAsync(),
                $"groups/{groupId}/members/{userId}",
                "DELETE"
            );
            return Task.CompletedTask;
        }, tenantId, $"removing member {userId} from group {groupId}");
    }

    public async Task RemoveGroupOwnerAsync(string groupId, string userId, Guid? tenantId = null) {
        await _exceptionHandler.HandleAsync(async () => {
            var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
            await _cacheHandler.ExecuteAsync(
                tenantId,
                () => graphClient.Groups[groupId].Owners[userId].Ref.DeleteAsync(),
                $"groups/{groupId}/owners/{userId}",
                "DELETE"
            );
            return Task.CompletedTask;
        }, tenantId, $"removing owner {userId} from group {groupId}");
    }
}
