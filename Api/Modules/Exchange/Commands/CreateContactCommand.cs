using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands;

public record CreateContactCommand(string TenantId, CreateContactDto CreateDto) : IRequest<CreateContactCommand, Task<string>>;
