using CIPP.Shared.DTOs.Identity;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Commands;

public record UpdateAuthenticationMethodConfigCommand(string TenantId, string MethodId, UpdateAuthenticationMethodDto UpdateDto) : IRequest<UpdateAuthenticationMethodConfigCommand, Task<AuthenticationMethodDto>>;
