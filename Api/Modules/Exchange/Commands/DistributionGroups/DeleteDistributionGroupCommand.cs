using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands.DistributionGroups;

public record DeleteDistributionGroupCommand(string TenantId, string GroupId) : IRequest<DeleteDistributionGroupCommand, Task>;
