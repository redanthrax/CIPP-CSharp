using CIPP.Shared.DTOs.Identity;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Identity.Commands;

public record SetUserSignInStateCommand(SetUserSignInStateDto UserSignInStateData) : IRequest<SetUserSignInStateCommand, Task<string>>;
