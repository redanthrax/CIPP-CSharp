using CIPP.Api.Modules.Identity.Interfaces;
using CIPP.Api.Modules.Identity.Queries;
using CIPP.Shared.DTOs.Identity;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Handlers;

public class GetGroupQueryHandler : IRequestHandler<GetGroupQuery, Task<GroupDto?>> {
    private readonly IGroupService _groupService;
    private readonly ILogger<GetGroupQueryHandler> _logger;

    public GetGroupQueryHandler(
        IGroupService groupService,
        ILogger<GetGroupQueryHandler> logger) {
        _groupService = groupService;
        _logger = logger;
    }

    public async Task<GroupDto?> Handle(GetGroupQuery request, CancellationToken cancellationToken) {
        try {
            _logger.LogInformation("Getting group {GroupId} for tenant {TenantId}", 
                request.GroupId, request.TenantId);
            
            return await _groupService.GetGroupAsync(request.TenantId, request.GroupId, cancellationToken);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to get group {GroupId} for tenant {TenantId}", 
                request.GroupId, request.TenantId);
            throw new InvalidOperationException($"Failed to get group: {ex.Message}", ex);
        }
    }
}