using CIPP.Shared.DTOs.ConditionalAccess;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.ConditionalAccess.Commands;

public record CreateConditionalAccessPolicyCommand(Guid TenantId, CreateConditionalAccessPolicyDto Policy)
    : IRequest<CreateConditionalAccessPolicyCommand, Task<ConditionalAccessPolicyDto>>;
