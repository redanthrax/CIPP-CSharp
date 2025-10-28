using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands.MailResources;

public record UpdateResourceBookingPolicyCommand(Guid TenantId, string Identity, UpdateResourceBookingPolicyDto UpdateDto)
    : IRequest<UpdateResourceBookingPolicyCommand, Task>;
