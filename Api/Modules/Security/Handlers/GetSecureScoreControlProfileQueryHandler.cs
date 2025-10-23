using CIPP.Api.Modules.Security.Interfaces;
using CIPP.Api.Modules.Security.Queries;
using CIPP.Shared.DTOs.Security;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Security.Handlers;

public class GetSecureScoreControlProfileQueryHandler : IRequestHandler<GetSecureScoreControlProfileQuery, Task<SecureScoreControlProfileDto?>> {
    private readonly ISecureScoreService _secureScoreService;

    public GetSecureScoreControlProfileQueryHandler(ISecureScoreService secureScoreService) {
        _secureScoreService = secureScoreService;
    }

    public async Task<SecureScoreControlProfileDto?> Handle(GetSecureScoreControlProfileQuery query, CancellationToken cancellationToken) {
        return await _secureScoreService.GetControlProfileAsync(query.TenantId, query.ControlName, cancellationToken);
    }
}
