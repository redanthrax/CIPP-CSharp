using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Commands.DistributionGroups;

public record CreateDistributionGroupCommand(string TenantId, CreateDistributionGroupDto Group) : IRequest<CreateDistributionGroupCommand, Task>;
