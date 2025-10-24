using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands;

public record UpdateAtpPolicyCommand(string TenantId, UpdateAtpPolicyDto UpdateDto) : IRequest<UpdateAtpPolicyCommand, Task>;
