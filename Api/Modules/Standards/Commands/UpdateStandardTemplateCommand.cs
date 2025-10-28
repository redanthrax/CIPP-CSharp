using CIPP.Shared.DTOs.Standards;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Standards.Commands;

public record UpdateStandardTemplateCommand(Guid TemplateId, UpdateStandardTemplateDto UpdateDto, string? ModifiedBy)
    : IRequest<UpdateStandardTemplateCommand, Task<StandardTemplateDto?>>;
