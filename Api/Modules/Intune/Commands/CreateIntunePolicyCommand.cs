using CIPP.Shared.DTOs.Intune;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Commands;

public record CreateIntunePolicyCommand(string TenantId, CreateIntunePolicyDto PolicyDto) 
    : IRequest<CreateIntunePolicyCommand, Task<IntunePolicyDto>>;
