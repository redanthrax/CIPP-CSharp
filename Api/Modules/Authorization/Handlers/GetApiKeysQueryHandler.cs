using CIPP.Api.Data;
using CIPP.Api.Modules.Authorization.Models;
using CIPP.Api.Modules.Authorization.Queries;
using CIPP.Shared.DTOs;
using DispatchR.Abstractions.Send;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Authorization.Handlers;

public class GetApiKeysQueryHandler : IRequestHandler<GetApiKeysQuery, Task<PagedResponse<ApiKey>>> {
    private readonly ApplicationDbContext _context;
    private readonly ILogger<GetApiKeysQueryHandler> _logger;

    public GetApiKeysQueryHandler(ApplicationDbContext context, ILogger<GetApiKeysQueryHandler> logger) {
        _context = context;
        _logger = logger;
    }

    public async Task<PagedResponse<ApiKey>> Handle(GetApiKeysQuery request, CancellationToken cancellationToken) {
        try {
            var apiKeys = await _context.GetEntitySet<ApiKey>()
                .Where(ak => ak.IsActive)
                .Include(ak => ak.ApiKeyRoles)
                .ThenInclude(akr => akr.Role)
                .OrderByDescending(ak => ak.CreatedAt)
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} active API keys", apiKeys.Count);

            return new PagedResponse<ApiKey> {
                Items = apiKeys,
                TotalCount = apiKeys.Count,
                PageNumber = 1,
                PageSize = apiKeys.Count
            };
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error retrieving API keys");
            throw;
        }
    }
}
