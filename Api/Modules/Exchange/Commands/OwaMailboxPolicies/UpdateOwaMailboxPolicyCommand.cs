using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands.OwaMailboxPolicies;

public record UpdateOwaMailboxPolicyCommand(Guid TenantId, string PolicyId, UpdateOwaMailboxPolicyDto UpdateDto)
    : IRequest<UpdateOwaMailboxPolicyCommand, Task>;
