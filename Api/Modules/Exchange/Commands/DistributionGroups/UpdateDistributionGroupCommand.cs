using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands.DistributionGroups;

public record UpdateDistributionGroupCommand(string TenantId, string GroupId, UpdateDistributionGroupDto Group) : IRequest<UpdateDistributionGroupCommand, Task>;
