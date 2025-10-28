using CIPP.Api.Modules.Standards.Commands;
using CIPP.Api.Modules.Standards.Interfaces;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Standards.Handlers;

public class DeleteStandardTemplateCommandHandler : IRequestHandler<DeleteStandardTemplateCommand, Task<bool>> {
    private readonly IStandardsService _standardsService;

    public DeleteStandardTemplateCommandHandler(IStandardsService standardsService) {
        _standardsService = standardsService;
    }

    public async Task<bool> Handle(DeleteStandardTemplateCommand request, CancellationToken cancellationToken = default) {
        return await _standardsService.DeleteTemplateAsync(request.TemplateId, cancellationToken);
    }
}
