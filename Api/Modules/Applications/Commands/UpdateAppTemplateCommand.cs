using CIPP.Shared.DTOs.Applications;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Commands;

public record UpdateAppTemplateCommand(
    Guid Id,
    UpdateAppTemplateDto UpdateDto
) : IRequest<UpdateAppTemplateCommand, Task<AppTemplateDto>>;
