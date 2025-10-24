using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries.DistributionGroups;

public record GetDistributionGroupMembersQuery(string TenantId, string GroupId, PagingParameters PagingParams) : IRequest<GetDistributionGroupMembersQuery, Task<PagedResponse<DistributionGroupMemberDto>>>;
