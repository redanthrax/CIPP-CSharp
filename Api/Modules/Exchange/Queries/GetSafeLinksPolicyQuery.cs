using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries;

public record GetSafeLinksPolicyQuery(string TenantId, string PolicyName) : IRequest<GetSafeLinksPolicyQuery, Task<SafeLinksPolicyDto?>>;
