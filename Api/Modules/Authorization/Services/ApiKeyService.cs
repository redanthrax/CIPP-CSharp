using System.Security.Cryptography;
using System.Text;
using CIPP.Api.Data;
using CIPP.Api.Modules.Authorization.Interfaces;
using CIPP.Api.Modules.Authorization.Models;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Authorization.Services;
public class ApiKeyService : IApiKeyService {
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ApiKeyService> _logger;

    public ApiKeyService(ApplicationDbContext context, ILogger<ApiKeyService> logger) {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> ValidateApiKeyAsync(string apiKey) {
        if (string.IsNullOrEmpty(apiKey)) {
            return false;
        }

        var keyHash = ComputeHash(apiKey);
        var storedKey = await _context.GetEntitySet<ApiKey>()
            .FirstOrDefaultAsync(k => k.KeyHash == keyHash && k.IsActive);

        if (storedKey == null) {
            return false;
        }

        if (storedKey.ExpiresAt.HasValue && storedKey.ExpiresAt < DateTime.UtcNow) {
            return false;
        }

        return true;
    }

    public async Task RecordApiKeyUsageAsync(string apiKey) {
        if (string.IsNullOrEmpty(apiKey)) {
            return;
        }

        try {
            var keyHash = ComputeHash(apiKey);
            var storedKey = await _context.GetEntitySet<ApiKey>()
                .FirstOrDefaultAsync(k => k.KeyHash == keyHash && k.IsActive);

            if (storedKey != null) {
                storedKey.LastUsedAt = DateTime.UtcNow;
                storedKey.UsageCount++;
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error recording API key usage");
        }
    }

    public async Task<string> CreateApiKeyAsync(string name, string description, string createdBy,
        DateTime? expiresAt = null, List<Guid>? roleIds = null) {
        var apiKey = GenerateApiKey();
        var keyHash = ComputeHash(apiKey);

        var apiKeyEntity = new ApiKey {
            Id = Guid.NewGuid(),
            Name = name,
            KeyHash = keyHash,
            Description = description,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy,
            ExpiresAt = expiresAt,
            IsActive = true
        };

        _context.GetEntitySet<ApiKey>().Add(apiKeyEntity);
        await _context.SaveChangesAsync();

        if (roleIds != null && roleIds.Any()) {
            foreach (var roleId in roleIds) {
                var apiKeyRole = new ApiKeyRole {
                    ApiKeyId = apiKeyEntity.Id,
                    RoleId = roleId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = createdBy
                };
                await _context.GetEntitySet<ApiKeyRole>().AddAsync(apiKeyRole);
            }
            await _context.SaveChangesAsync();
        }

        return apiKey;
    }

    public async Task<bool> RevokeApiKeyAsync(string name) {
        var apiKey = await _context.GetEntitySet<ApiKey>()
            .FirstOrDefaultAsync(k => k.Name == name && k.IsActive);

        if (apiKey == null) {
            return false;
        }

        apiKey.IsActive = false;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<ApiKey>> GetActiveApiKeysAsync() {
        return await _context.GetEntitySet<ApiKey>()
            .Where(k => k.IsActive)
            .Include(k => k.ApiKeyRoles)
            .ThenInclude(akr => akr.Role)
            .OrderByDescending(k => k.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> AssignRoleToApiKeyAsync(Guid apiKeyId, Guid roleId, string assignedBy) {
        try {
            var existingAssignment = await _context.GetEntitySet<ApiKeyRole>()
                .FirstOrDefaultAsync(akr => akr.ApiKeyId == apiKeyId && akr.RoleId == roleId);
            
            if (existingAssignment != null) {
                return true; 
            }
            
            var apiKeyRole = new ApiKeyRole {
                ApiKeyId = apiKeyId,
                RoleId = roleId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = assignedBy
            };
            
            await _context.GetEntitySet<ApiKeyRole>().AddAsync(apiKeyRole);
            await _context.SaveChangesAsync();
            
            return true;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error assigning role {RoleId} to API key {ApiKeyId}", roleId, apiKeyId);
            return false;
        }
    }

    public async Task<bool> RemoveRoleFromApiKeyAsync(Guid apiKeyId, Guid roleId) {
        try {
            var apiKeyRole = await _context.GetEntitySet<ApiKeyRole>()
                .FirstOrDefaultAsync(akr => akr.ApiKeyId == apiKeyId && akr.RoleId == roleId);
            
            if (apiKeyRole == null) {
                return true; 
            }
            
            _context.GetEntitySet<ApiKeyRole>().Remove(apiKeyRole);
            await _context.SaveChangesAsync();
            
            return true;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error removing role {RoleId} from API key {ApiKeyId}", roleId, apiKeyId);
            return false;
        }
    }

    public async Task<List<string>> GetApiKeyPermissionsAsync(string apiKey) {
        try {
            var keyHash = ComputeHash(apiKey);
            
            var permissions = await _context.GetEntitySet<ApiKey>()
                .Where(k => k.KeyHash == keyHash && k.IsActive)
                .SelectMany(k => k.ApiKeyRoles)
                .Where(akr => akr.ExpiresAt == null || akr.ExpiresAt > DateTime.UtcNow)
                .SelectMany(akr => akr.Role.RolePermissions)
                .Select(rp => rp.Permission.Name)
                .Distinct()
                .ToListAsync();
            
            return permissions;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error getting permissions for API key");
            return new List<string>();
        }
    }

    public async Task<bool> ApiKeyHasPermissionAsync(string apiKey, string permissionName) {
        try {
            var keyHash = ComputeHash(apiKey);
            
            var hasPermission = await _context.GetEntitySet<ApiKey>()
                .Where(k => k.KeyHash == keyHash && k.IsActive)
                .SelectMany(k => k.ApiKeyRoles)
                .Where(akr => akr.ExpiresAt == null || akr.ExpiresAt > DateTime.UtcNow)
                .SelectMany(akr => akr.Role.RolePermissions)
                .AnyAsync(rp => rp.Permission.Name == permissionName && rp.Permission.IsActive);
            
            return hasPermission;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error checking permission {Permission} for API key", permissionName);
            return false;
        }
    }

    private static string GenerateApiKey() {
        const int keyLength = 32;
        var randomBytes = new byte[keyLength];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    private static string ComputeHash(string input) {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
