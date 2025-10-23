using CIPP.Shared.DTOs.Intune;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Queries;

public record GetIntuneAppsQuery(string TenantId) 
    : IRequest<GetIntuneAppsQuery, Task<List<IntuneAppDto>>>;
