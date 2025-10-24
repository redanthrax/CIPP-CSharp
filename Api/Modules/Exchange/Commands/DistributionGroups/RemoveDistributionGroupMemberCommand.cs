using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands.DistributionGroups;

public record RemoveDistributionGroupMemberCommand(string TenantId, string GroupId, string MemberEmail) : IRequest<RemoveDistributionGroupMemberCommand, Task>;
