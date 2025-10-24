using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries.DistributionGroups;

public record GetDistributionGroupQuery(string TenantId, string GroupId) : IRequest<GetDistributionGroupQuery, Task<DistributionGroupDto?>>;
