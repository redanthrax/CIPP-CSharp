using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands;

public record UpdateLitigationHoldCommand(string TenantId, string MailboxId, LitigationHoldDto HoldDto) : IRequest<UpdateLitigationHoldCommand, Task>;
