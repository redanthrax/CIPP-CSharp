using CIPP.Api.Modules.Standards.Interfaces;
using CIPP.Api.Modules.Standards.Queries;
using CIPP.Shared.DTOs.Standards;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Standards.Handlers;

public class GetBpaReportQueryHandler : IRequestHandler<GetBpaReportQuery, Task<BpaReportDto>> {
    private readonly IBpaService _bpaService;

    public GetBpaReportQueryHandler(IBpaService bpaService) {
        _bpaService = bpaService;
    }

    public async Task<BpaReportDto> Handle(GetBpaReportQuery request, CancellationToken cancellationToken = default) {
        return await _bpaService.GetReportAsync(request.TenantId, request.Category, cancellationToken);
    }
}
