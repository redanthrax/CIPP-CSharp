using CIPP.Shared.DTOs.Standards;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Standards.Commands;

public record DeployStandardCommand(DeployStandardDto DeployDto, string? ExecutedBy)
    : IRequest<DeployStandardCommand, Task<List<StandardResultDto>>>;
