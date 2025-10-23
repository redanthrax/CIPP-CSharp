using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.ConditionalAccess.Commands;

public record DeleteConditionalAccessTemplateCommand(Guid Id) 
    : IRequest<DeleteConditionalAccessTemplateCommand, Task>;
