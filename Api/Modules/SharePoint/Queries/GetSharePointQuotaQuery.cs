using CIPP.Shared.DTOs.SharePoint;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.SharePoint.Queries;

public record GetSharePointQuotaQuery(string TenantId) : IRequest<GetSharePointQuotaQuery, Task<SharePointQuotaDto>>;
