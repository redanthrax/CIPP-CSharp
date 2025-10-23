using CIPP.Shared.DTOs.SharePoint;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.SharePoint.Queries;

public record GetSharePointSettingsQuery(string TenantId) : IRequest<GetSharePointSettingsQuery, Task<SharePointSettingsDto>>;
