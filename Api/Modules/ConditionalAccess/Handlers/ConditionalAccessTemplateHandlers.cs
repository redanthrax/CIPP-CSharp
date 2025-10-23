using CIPP.Api.Modules.ConditionalAccess.Commands;
using CIPP.Api.Modules.ConditionalAccess.Interfaces;
using CIPP.Api.Modules.ConditionalAccess.Queries;
using CIPP.Shared.DTOs.ConditionalAccess;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.ConditionalAccess.Handlers;

public class GetConditionalAccessTemplateQueryHandler : IRequestHandler<GetConditionalAccessTemplateQuery, Task<ConditionalAccessTemplateDto?>> {
    private readonly IConditionalAccessTemplateService _templateService;

    public GetConditionalAccessTemplateQueryHandler(IConditionalAccessTemplateService templateService) {
        _templateService = templateService;
    }

    public async Task<ConditionalAccessTemplateDto?> Handle(GetConditionalAccessTemplateQuery query, CancellationToken cancellationToken) {
        return await _templateService.GetTemplateAsync(query.Id, cancellationToken);
    }
}

public class CreateConditionalAccessTemplateCommandHandler : IRequestHandler<CreateConditionalAccessTemplateCommand, Task<ConditionalAccessTemplateDto>> {
    private readonly IConditionalAccessTemplateService _templateService;

    public CreateConditionalAccessTemplateCommandHandler(IConditionalAccessTemplateService templateService) {
        _templateService = templateService;
    }

    public async Task<ConditionalAccessTemplateDto> Handle(CreateConditionalAccessTemplateCommand command, CancellationToken cancellationToken) {
        return await _templateService.CreateTemplateAsync(command.CreateDto, cancellationToken);
    }
}

public class UpdateConditionalAccessTemplateCommandHandler : IRequestHandler<UpdateConditionalAccessTemplateCommand, Task<ConditionalAccessTemplateDto>> {
    private readonly IConditionalAccessTemplateService _templateService;

    public UpdateConditionalAccessTemplateCommandHandler(IConditionalAccessTemplateService templateService) {
        _templateService = templateService;
    }

    public async Task<ConditionalAccessTemplateDto> Handle(UpdateConditionalAccessTemplateCommand command, CancellationToken cancellationToken) {
        return await _templateService.UpdateTemplateAsync(command.Id, command.UpdateDto, cancellationToken);
    }
}

public class DeleteConditionalAccessTemplateCommandHandler : IRequestHandler<DeleteConditionalAccessTemplateCommand, Task> {
    private readonly IConditionalAccessTemplateService _templateService;

    public DeleteConditionalAccessTemplateCommandHandler(IConditionalAccessTemplateService templateService) {
        _templateService = templateService;
    }

    public async Task Handle(DeleteConditionalAccessTemplateCommand command, CancellationToken cancellationToken) {
        await _templateService.DeleteTemplateAsync(command.Id, cancellationToken);
    }
}

public class DeployConditionalAccessTemplateCommandHandler : IRequestHandler<DeployConditionalAccessTemplateCommand, Task<ConditionalAccessPolicyDto>> {
    private readonly IConditionalAccessTemplateService _templateService;

    public DeployConditionalAccessTemplateCommandHandler(IConditionalAccessTemplateService templateService) {
        _templateService = templateService;
    }

    public async Task<ConditionalAccessPolicyDto> Handle(DeployConditionalAccessTemplateCommand command, CancellationToken cancellationToken) {
        return await _templateService.DeployTemplateAsync(command.DeployDto, cancellationToken);
    }
}
