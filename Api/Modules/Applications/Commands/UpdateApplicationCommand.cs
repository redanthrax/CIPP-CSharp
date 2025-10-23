using CIPP.Shared.DTOs.Applications;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Commands;

public record UpdateApplicationCommand(
    string TenantId,
    string ApplicationId,
    UpdateApplicationDto Application
) : IRequest<UpdateApplicationCommand, Task<ApplicationDto>>;
