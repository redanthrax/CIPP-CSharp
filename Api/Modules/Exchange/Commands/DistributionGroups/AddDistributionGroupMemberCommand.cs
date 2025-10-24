using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands.DistributionGroups;

public record AddDistributionGroupMemberCommand(string TenantId, string GroupId, string MemberEmail) : IRequest<AddDistributionGroupMemberCommand, Task>;
