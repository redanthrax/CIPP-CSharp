using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Standards.Commands;

public record DeleteStandardTemplateCommand(Guid TemplateId)
    : IRequest<DeleteStandardTemplateCommand, Task<bool>>;
