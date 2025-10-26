using CIPP.Shared.DTOs.ConditionalAccess;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.ConditionalAccess.Commands;

public record DeployConditionalAccessTemplateCommand(Guid TenantId, DeployConditionalAccessTemplateDto DeployDto)
    : IRequest<DeployConditionalAccessTemplateCommand, Task<ConditionalAccessPolicyDto>>;
