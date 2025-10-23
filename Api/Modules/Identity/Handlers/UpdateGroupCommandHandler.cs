using CIPP.Api.Modules.Identity.Commands;
using CIPP.Api.Modules.Identity.Interfaces;
using CIPP.Shared.DTOs.Identity;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Handlers;

public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand, Task<GroupDto>> {
    private readonly IGroupService _groupService;
    private readonly ILogger<UpdateGroupCommandHandler> _logger;

    public UpdateGroupCommandHandler(IGroupService groupService, ILogger<UpdateGroupCommandHandler> logger) {
        _groupService = groupService;
        _logger = logger;
    }

    public async Task<GroupDto> Handle(UpdateGroupCommand request, CancellationToken cancellationToken) {
        try {
            _logger.LogInformation("Updating group {GroupId} for tenant {TenantId}", request.GroupId, request.TenantId);
            return await _groupService.UpdateGroupAsync(request.TenantId, request.GroupId, request.GroupData, cancellationToken);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to update group {GroupId}", request.GroupId);
            throw new InvalidOperationException($"Failed to update group: {ex.Message}", ex);
        }
    }
}