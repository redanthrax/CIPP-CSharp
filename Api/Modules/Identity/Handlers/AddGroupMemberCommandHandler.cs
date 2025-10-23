using CIPP.Api.Modules.Identity.Commands;
using CIPP.Api.Modules.Identity.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Handlers;

public class AddGroupMemberCommandHandler : IRequestHandler<AddGroupMemberCommand, Task> {
    private readonly IGroupService _groupService;
    private readonly ILogger<AddGroupMemberCommandHandler> _logger;

    public AddGroupMemberCommandHandler(IGroupService groupService, ILogger<AddGroupMemberCommandHandler> logger) {
        _groupService = groupService;
        _logger = logger;
    }

    public async Task Handle(AddGroupMemberCommand request, CancellationToken cancellationToken) {
        try {
            _logger.LogInformation("Adding member {UserId} to group {GroupId} for tenant {TenantId}", 
                request.MemberData.UserId, request.GroupId, request.TenantId);
            await _groupService.AddGroupMemberAsync(request.TenantId, request.GroupId, request.MemberData, cancellationToken);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to add member to group {GroupId}", request.GroupId);
            throw new InvalidOperationException($"Failed to add group member: {ex.Message}", ex);
        }
    }
}