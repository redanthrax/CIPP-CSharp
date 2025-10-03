using CIPP.Api.Modules.Authorization.Commands;
using CIPP.Api.Modules.Authorization.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Authorization.Handlers;

public class AssignRoleToApiKeyCommandHandler : IRequestHandler<AssignRoleToApiKeyCommand, Task<bool>> {
    private readonly IApiKeyService _apiKeyService;
    private readonly ILogger<AssignRoleToApiKeyCommandHandler> _logger;

    public AssignRoleToApiKeyCommandHandler(IApiKeyService apiKeyService, ILogger<AssignRoleToApiKeyCommandHandler> logger) {
        _apiKeyService = apiKeyService;
        _logger = logger;
    }

    public async Task<bool> Handle(AssignRoleToApiKeyCommand request, CancellationToken cancellationToken) {
        try {
            var success = await _apiKeyService.AssignRoleToApiKeyAsync(request.ApiKeyId, request.RoleId, request.AssignedBy);
            
            if (success) {
                _logger.LogInformation("Assigned role {RoleId} to API key {ApiKeyId}", request.RoleId, request.ApiKeyId);
            } else {
                _logger.LogWarning("Failed to assign role {RoleId} to API key {ApiKeyId}", request.RoleId, request.ApiKeyId);
            }

            return success;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error assigning role {RoleId} to API key {ApiKeyId}", request.RoleId, request.ApiKeyId);
            throw;
        }
    }
}
