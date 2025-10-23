using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries;

public record GetContactQuery(string TenantId, string ContactId) : IRequest<GetContactQuery, Task<ContactDetailsDto?>>;
