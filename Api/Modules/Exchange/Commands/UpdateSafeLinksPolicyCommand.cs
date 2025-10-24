using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands;

public record UpdateSafeLinksPolicyCommand(string TenantId, string PolicyName, UpdateSafeLinksPolicyDto UpdateDto) : IRequest<UpdateSafeLinksPolicyCommand, Task>;
