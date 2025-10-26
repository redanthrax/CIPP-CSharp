using Microsoft.Graph.Beta;
using Microsoft.Graph.Beta.Models;
using CIPP.Shared.DTOs;

namespace CIPP.Api.Modules.MsGraph.Services;

public class GraphUserService {
    private readonly MicrosoftGraphService _graphService;
    private readonly CachedGraphRequestHandler _cacheHandler;
    private readonly GraphExceptionHandler _exceptionHandler;

    public GraphUserService(MicrosoftGraphService graphService, CachedGraphRequestHandler cacheHandler, GraphExceptionHandler exceptionHandler) {
        _graphService = graphService;
        _cacheHandler = cacheHandler;
        _exceptionHandler = exceptionHandler;
    }

    public async Task<UserCollectionResponse?> ListUsersAsync(Guid? tenantId = null, string? filter = null, string[]? select = null, string? search = null, PagingParameters? paging = null) {
        return await _exceptionHandler.HandleAsync(async () => {
            var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
            paging ??= new PagingParameters();
            
            return await _cacheHandler.ExecuteAsync(
                tenantId,
                () => graphClient.Users.GetAsync(requestConfiguration => {
                    if (!string.IsNullOrEmpty(filter))
                        requestConfiguration.QueryParameters.Filter = filter;
                    if (select != null && select.Length > 0)
                        requestConfiguration.QueryParameters.Select = select;
                    if (!string.IsNullOrEmpty(search)) {
                        requestConfiguration.QueryParameters.Search = search;
                        requestConfiguration.Headers.Add("ConsistencyLevel", "eventual");
                    }
                    requestConfiguration.QueryParameters.Top = paging.PageSize;
                }),
                "users",
                "GET",
                new { filter, select = select != null ? string.Join(",", select) : null, search, pageNumber = paging.PageNumber, pageSize = paging.PageSize, skipToken = paging.SkipToken }
            );
        }, tenantId, "listing users");
    }

    public async Task<User?> GetUserAsync(string userId, Guid? tenantId = null) {
        return await _exceptionHandler.HandleAsync(async () => {
            var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
            return await _cacheHandler.ExecuteAsync(
                tenantId,
                () => graphClient.Users[userId].GetAsync(),
                $"users/{userId}",
                "GET"
            );
        }, tenantId, $"getting user {userId}");
    }

    public async Task<User?> CreateUserAsync(User user, Guid? tenantId = null) {
        return await _exceptionHandler.HandleAsync(async () => {
            var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
            return await _cacheHandler.ExecuteAsync(
                tenantId,
                () => graphClient.Users.PostAsync(user),
                "users",
                "POST"
            );
        }, tenantId, "creating user");
    }

    public async Task<User?> UpdateUserAsync(string userId, User user, Guid? tenantId = null) {
        return await _exceptionHandler.HandleAsync(async () => {
            var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
            return await _cacheHandler.ExecuteAsync(
                tenantId,
                () => graphClient.Users[userId].PatchAsync(user),
                $"users/{userId}",
                "PATCH"
            );
        }, tenantId, $"updating user {userId}");
    }

    public async Task DeleteUserAsync(string userId, Guid? tenantId = null) {
        await _exceptionHandler.HandleAsync(async () => {
            var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
            await _cacheHandler.ExecuteAsync(
                tenantId,
                () => graphClient.Users[userId].DeleteAsync(),
                $"users/{userId}",
                "DELETE"
            );
            return Task.CompletedTask;
        }, tenantId, $"deleting user {userId}");
    }

    public async Task<AuthenticationMethodCollectionResponse?> GetUserAuthenticationMethodsAsync(string userId, Guid? tenantId = null) {
        return await _exceptionHandler.HandleAsync(async () => {
            var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
            return await _cacheHandler.ExecuteAsync(
                tenantId,
                () => graphClient.Users[userId].Authentication.Methods.GetAsync(),
                $"users/{userId}/authentication/methods",
                "GET"
            );
        }, tenantId, $"getting user authentication methods for {userId}");
    }

    public async Task<DirectoryObjectCollectionResponse?> GetUserMemberOfAsync(string userId, Guid? tenantId = null) {
        return await _exceptionHandler.HandleAsync(async () => {
            var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
            return await _cacheHandler.ExecuteAsync(
                tenantId,
                () => graphClient.Users[userId].MemberOf.GetAsync(),
                $"users/{userId}/memberOf",
                "GET"
            );
        }, tenantId, $"getting user member of for {userId}");
    }

    public async Task<DirectoryObjectCollectionResponse?> GetUserRegisteredDevicesAsync(string userId, Guid? tenantId = null) {
        return await _exceptionHandler.HandleAsync(async () => {
            var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
            return await _cacheHandler.ExecuteAsync(
                tenantId,
                () => graphClient.Users[userId].RegisteredDevices.GetAsync(),
                $"users/{userId}/registeredDevices",
                "GET"
            );
        }, tenantId, $"getting user registered devices for {userId}");
    }

    public async Task<DirectoryObjectCollectionResponse?> GetUserOwnedDevicesAsync(string userId, Guid? tenantId = null) {
        return await _exceptionHandler.HandleAsync(async () => {
            var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
            return await _cacheHandler.ExecuteAsync(
                tenantId,
                () => graphClient.Users[userId].OwnedDevices.GetAsync(),
                $"users/{userId}/ownedDevices",
                "GET"
            );
        }, tenantId, $"getting user owned devices for {userId}");
    }

    public async Task<DirectoryObject?> GetUserManagerAsync(string userId, Guid? tenantId = null) {
        return await _exceptionHandler.HandleAsync(async () => {
            var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
            return await _cacheHandler.ExecuteAsync(
                tenantId,
                () => graphClient.Users[userId].Manager.GetAsync(),
                $"users/{userId}/manager",
                "GET"
            );
        }, tenantId, $"getting user manager for {userId}");
    }
}
