using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands;

public record UpdateAntiSpamPolicyCommand(
    string TenantId,
    string PolicyId,
    UpdateHostedContentFilterPolicyDto UpdateDto
) : IRequest<UpdateAntiSpamPolicyCommand, Task>;
