using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries;

public record GetMailboxQuery(string TenantId, string UserId) : IRequest<GetMailboxQuery, Task<MailboxDetailsDto?>>;
