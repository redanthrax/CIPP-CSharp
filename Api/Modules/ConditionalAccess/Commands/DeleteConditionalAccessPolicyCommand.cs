using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.ConditionalAccess.Commands;

public record DeleteConditionalAccessPolicyCommand(
    string TenantId,
    string PolicyId
) : IRequest<DeleteConditionalAccessPolicyCommand, Task>;
