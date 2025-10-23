using CIPP.Shared.DTOs.ConditionalAccess;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.ConditionalAccess.Commands;

public record DeployConditionalAccessTemplateCommand(DeployConditionalAccessTemplateDto DeployDto) 
    : IRequest<DeployConditionalAccessTemplateCommand, Task<ConditionalAccessPolicyDto>>;
