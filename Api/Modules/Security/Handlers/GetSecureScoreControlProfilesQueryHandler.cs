using CIPP.Api.Modules.Security.Interfaces;
using CIPP.Api.Modules.Security.Queries;
using CIPP.Shared.DTOs.Security;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Security.Handlers;

public class GetSecureScoreControlProfilesQueryHandler : IRequestHandler<GetSecureScoreControlProfilesQuery, Task<List<SecureScoreControlProfileDto>>> {
    private readonly ISecureScoreService _secureScoreService;

    public GetSecureScoreControlProfilesQueryHandler(ISecureScoreService secureScoreService) {
        _secureScoreService = secureScoreService;
    }

    public async Task<List<SecureScoreControlProfileDto>> Handle(GetSecureScoreControlProfilesQuery query, CancellationToken cancellationToken) {
        return await _secureScoreService.GetControlProfilesAsync(query.TenantId, cancellationToken);
    }
}
