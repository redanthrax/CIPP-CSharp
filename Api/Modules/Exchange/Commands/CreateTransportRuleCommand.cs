using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands;

public record CreateTransportRuleCommand(string TenantId, CreateTransportRuleDto CreateDto) : IRequest<CreateTransportRuleCommand, Task<string>>;
