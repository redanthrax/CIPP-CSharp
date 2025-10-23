using CIPP.Shared.DTOs.Applications;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Applications.Queries;

public record GetPermissionSetQuery(
    Guid Id
) : IRequest<GetPermissionSetQuery, Task<PermissionSetDto?>>;
