using CIPP.Shared.DTOs.Security;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Security.Queries;

public record GetSecureScoreControlProfilesQuery(
    string TenantId
) : IRequest<GetSecureScoreControlProfilesQuery, Task<List<SecureScoreControlProfileDto>>>;
