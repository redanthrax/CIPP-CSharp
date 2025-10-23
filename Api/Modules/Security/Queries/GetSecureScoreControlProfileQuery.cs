using CIPP.Shared.DTOs.Security;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Security.Queries;

public record GetSecureScoreControlProfileQuery(
    string TenantId,
    string ControlName
) : IRequest<GetSecureScoreControlProfileQuery, Task<SecureScoreControlProfileDto?>>;
