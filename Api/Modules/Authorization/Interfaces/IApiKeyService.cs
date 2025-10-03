namespace CIPP.Api.Modules.Authorization.Interfaces;
public interface IApiKeyService {
    Task<bool> ValidateApiKeyAsync(string apiKey);
    Task RecordApiKeyUsageAsync(string apiKey);
    Task<string> CreateApiKeyAsync(string name, string description, string createdBy, DateTime? expiresAt = null, List<Guid>? roleIds = null);
    Task<bool> RevokeApiKeyAsync(string name);
    Task<IEnumerable<Models.ApiKey>> GetActiveApiKeysAsync();
    Task<bool> AssignRoleToApiKeyAsync(Guid apiKeyId, Guid roleId, string assignedBy);
    Task<bool> RemoveRoleFromApiKeyAsync(Guid apiKeyId, Guid roleId);
    Task<List<string>> GetApiKeyPermissionsAsync(string apiKey);
    Task<bool> ApiKeyHasPermissionAsync(string apiKey, string permissionName);
}
