using CIPP.Shared.DTOs.Identity;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Commands;

public record CreateUserCommand(
    CreateUserDto UserData
) : IRequest<CreateUserCommand, Task<UserDto>>;