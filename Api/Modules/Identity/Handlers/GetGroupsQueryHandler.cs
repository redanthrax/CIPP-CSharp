using CIPP.Api.Modules.Identity.Interfaces;
using CIPP.Api.Modules.Identity.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Handlers;

public class GetGroupsQueryHandler : IRequestHandler<GetGroupsQuery, Task<PagedResponse<GroupDto>>> {
    private readonly IGroupService _groupService;
    private readonly ILogger<GetGroupsQueryHandler> _logger;

    public GetGroupsQueryHandler(
        IGroupService groupService,
        ILogger<GetGroupsQueryHandler> logger) {
        _groupService = groupService;
        _logger = logger;
    }

    public async Task<PagedResponse<GroupDto>> Handle(GetGroupsQuery request, CancellationToken cancellationToken) {
        try {
            _logger.LogInformation("Getting groups for tenant {TenantId}", request.TenantId);
            
            var result = await _groupService.GetGroupsAsync(request.TenantId, request.Paging, cancellationToken);
            return result;
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to get groups for tenant {TenantId}", request.TenantId);
            return new PagedResponse<GroupDto> {
                Items = new List<GroupDto>(),
                TotalCount = 0,
                PageNumber = request.Paging?.PageNumber ?? 1,
                PageSize = request.Paging?.PageSize ?? 50
            };
        }
    }
}
