using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands;

public record UpdateContactCommand(string TenantId, string ContactId, UpdateContactDto UpdateDto) : IRequest<UpdateContactCommand, Task>;
