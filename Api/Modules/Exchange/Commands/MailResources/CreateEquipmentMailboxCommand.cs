using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands.MailResources;

public record CreateEquipmentMailboxCommand(Guid TenantId, CreateEquipmentMailboxDto CreateDto)
    : IRequest<CreateEquipmentMailboxCommand, Task<string>>;
