using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Authorization.Commands;

public record RevokeApiKeyCommand(
    string Name
) : IRequest<RevokeApiKeyCommand, Task<bool>>;
