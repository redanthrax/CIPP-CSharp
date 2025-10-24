using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries;

public record GetAtpPolicyQuery(string TenantId) : IRequest<GetAtpPolicyQuery, Task<AtpPolicyDto?>>;
