using CIPP.Shared.DTOs.Intune;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Intune.Commands;

public record UpdateIntunePolicyCommand(string TenantId, string PolicyId, UpdateIntunePolicyDto PolicyDto) 
    : IRequest<UpdateIntunePolicyCommand, Task<IntunePolicyDto>>;
