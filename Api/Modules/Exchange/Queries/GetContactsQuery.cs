using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries;

public record GetContactsQuery(string TenantId) : IRequest<GetContactsQuery, Task<List<ContactDto>>>;
