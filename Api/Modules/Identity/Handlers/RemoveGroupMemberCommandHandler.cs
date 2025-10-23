using CIPP.Api.Modules.Identity.Commands;
using CIPP.Api.Modules.Identity.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Handlers;

public class RemoveGroupMemberCommandHandler : IRequestHandler<RemoveGroupMemberCommand, Task> {
    private readonly IGroupService _groupService;
    private readonly ILogger<RemoveGroupMemberCommandHandler> _logger;

    public RemoveGroupMemberCommandHandler(IGroupService groupService, ILogger<RemoveGroupMemberCommandHandler> logger) {
        _groupService = groupService;
        _logger = logger;
    }

    public async Task Handle(RemoveGroupMemberCommand request, CancellationToken cancellationToken) {
        try {
            _logger.LogInformation("Removing member {UserId} from group {GroupId} for tenant {TenantId}", 
                request.UserId, request.GroupId, request.TenantId);
            await _groupService.RemoveGroupMemberAsync(request.TenantId, request.GroupId, request.UserId, cancellationToken);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to remove member from group {GroupId}", request.GroupId);
            throw new InvalidOperationException($"Failed to remove group member: {ex.Message}", ex);
        }
    }
}