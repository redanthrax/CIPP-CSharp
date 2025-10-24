using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries;

public record GetAntiSpamPolicyQuery(string TenantId, string PolicyId) : IRequest<GetAntiSpamPolicyQuery, Task<HostedContentFilterPolicyDto?>>;
