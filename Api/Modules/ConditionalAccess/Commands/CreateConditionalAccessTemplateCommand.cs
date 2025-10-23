using CIPP.Shared.DTOs.ConditionalAccess;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.ConditionalAccess.Commands;

public record CreateConditionalAccessTemplateCommand(CreateConditionalAccessTemplateDto CreateDto) 
    : IRequest<CreateConditionalAccessTemplateCommand, Task<ConditionalAccessTemplateDto>>;
