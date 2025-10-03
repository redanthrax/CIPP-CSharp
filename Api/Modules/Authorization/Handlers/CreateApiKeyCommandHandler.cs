using CIPP.Api.Data;
using CIPP.Api.Modules.Authorization.Commands;
using CIPP.Api.Modules.Authorization.Models;
using CIPP.Api.Modules.Authorization.Interfaces;
using DispatchR.Abstractions.Send;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Authorization.Handlers;

public class CreateApiKeyCommandHandler : IRequestHandler<CreateApiKeyCommand, Task<(ApiKey Entity, string GeneratedKey)>> {
    private readonly IApiKeyService _apiKeyService;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CreateApiKeyCommandHandler> _logger;

    public CreateApiKeyCommandHandler(
        IApiKeyService apiKeyService, 
        ApplicationDbContext context,
        ILogger<CreateApiKeyCommandHandler> logger) {
        _apiKeyService = apiKeyService;
        _context = context;
        _logger = logger;
    }

    public async Task<(ApiKey Entity, string GeneratedKey)> Handle(CreateApiKeyCommand request, CancellationToken cancellationToken) {
        try {
            var generatedKey = await _apiKeyService.CreateApiKeyAsync(
                request.Name,
                request.Description ?? string.Empty,
                request.CreatedBy,
                request.ExpiresAt,
                request.RoleIds
            );

            var apiKeyEntity = await _context.GetEntitySet<ApiKey>()
                .Include(ak => ak.ApiKeyRoles)
                .ThenInclude(akr => akr.Role)
                .FirstAsync(ak => ak.Name == request.Name, cancellationToken);

            _logger.LogInformation("Created API key {Name} with ID {Id}", request.Name, apiKeyEntity.Id);

            return (apiKeyEntity, generatedKey);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error creating API key {Name}", request.Name);
            throw;
        }
    }
}
