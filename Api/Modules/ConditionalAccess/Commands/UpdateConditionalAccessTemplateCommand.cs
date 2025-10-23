using CIPP.Shared.DTOs.ConditionalAccess;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.ConditionalAccess.Commands;

public record UpdateConditionalAccessTemplateCommand(Guid Id, UpdateConditionalAccessTemplateDto UpdateDto) 
    : IRequest<UpdateConditionalAccessTemplateCommand, Task<ConditionalAccessTemplateDto>>;
