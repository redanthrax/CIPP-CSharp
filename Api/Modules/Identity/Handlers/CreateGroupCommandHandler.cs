using CIPP.Api.Modules.Identity.Commands;
using CIPP.Api.Modules.Identity.Interfaces;
using CIPP.Shared.DTOs.Identity;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Handlers;

public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, Task<GroupDto>> {
    private readonly IGroupService _groupService;
    private readonly ILogger<CreateGroupCommandHandler> _logger;

    public CreateGroupCommandHandler(
        IGroupService groupService,
        ILogger<CreateGroupCommandHandler> logger) {
        _groupService = groupService;
        _logger = logger;
    }

    public async Task<GroupDto> Handle(CreateGroupCommand request, CancellationToken cancellationToken) {
        try {
            _logger.LogInformation("Creating group {DisplayName} for tenant {TenantId}", 
                request.GroupData.DisplayName, request.GroupData.TenantId);
            
            return await _groupService.CreateGroupAsync(request.GroupData, cancellationToken);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to create group {DisplayName}", request.GroupData.DisplayName);
            throw new InvalidOperationException($"Failed to create group: {ex.Message}", ex);
        }
    }
}