using CIPP.Shared.DTOs.Applications;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Commands;

public record CreateApplicationCredentialCommand(
    CreateApplicationCredentialDto CreateCredentialDto
) : IRequest<CreateApplicationCredentialCommand, Task<ApplicationCredentialDto>>;
