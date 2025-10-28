using CIPP.Api.Modules.Standards.Interfaces;
using CIPP.Api.Modules.Standards.Queries;
using CIPP.Shared.DTOs.Standards;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Standards.Handlers;

public class GetComplianceScoreQueryHandler : IRequestHandler<GetComplianceScoreQuery, Task<ComplianceScoreDto>> {
    private readonly IBpaService _bpaService;

    public GetComplianceScoreQueryHandler(IBpaService bpaService) {
        _bpaService = bpaService;
    }

    public async Task<ComplianceScoreDto> Handle(GetComplianceScoreQuery request, CancellationToken cancellationToken = default) {
        return await _bpaService.GetComplianceScoreAsync(request.TenantId, cancellationToken);
    }
}
