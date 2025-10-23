using CIPP.Shared.DTOs.Applications;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Queries;

public record GetApplicationQuery(
    string TenantId,
    string ApplicationId
) : IRequest<GetApplicationQuery, Task<ApplicationDto?>>;
