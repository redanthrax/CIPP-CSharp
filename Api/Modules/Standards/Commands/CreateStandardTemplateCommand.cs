using CIPP.Shared.DTOs.Standards;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Standards.Commands;

public record CreateStandardTemplateCommand(CreateStandardTemplateDto CreateDto, string? CreatedBy)
    : IRequest<CreateStandardTemplateCommand, Task<StandardTemplateDto>>;
