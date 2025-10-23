using CIPP.Shared.DTOs.Applications;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Commands;

public record DeleteApplicationCredentialCommand(
    DeleteApplicationCredentialDto DeleteCredentialDto
) : IRequest<DeleteApplicationCredentialCommand, Task>;
