using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands;

public record UpdateSafeAttachmentPolicyCommand(string TenantId, string PolicyName, UpdateSafeAttachmentsPolicyDto UpdateDto) : IRequest<UpdateSafeAttachmentPolicyCommand, Task>;
