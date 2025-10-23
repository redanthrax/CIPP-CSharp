using CIPP.Api.Modules.Identity.Commands;
using CIPP.Api.Modules.Identity.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Handlers;

public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand, Task> {
    private readonly IGroupService _groupService;
    private readonly ILogger<DeleteGroupCommandHandler> _logger;

    public DeleteGroupCommandHandler(IGroupService groupService, ILogger<DeleteGroupCommandHandler> logger) {
        _groupService = groupService;
        _logger = logger;
    }

    public async Task Handle(DeleteGroupCommand request, CancellationToken cancellationToken) {
        try {
            _logger.LogInformation("Deleting group {GroupId} for tenant {TenantId}", request.GroupId, request.TenantId);
            await _groupService.DeleteGroupAsync(request.TenantId, request.GroupId, cancellationToken);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to delete group {GroupId}", request.GroupId);
            throw new InvalidOperationException($"Failed to delete group: {ex.Message}", ex);
        }
    }
}