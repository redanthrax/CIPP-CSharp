using CIPP.Api.Modules.Tenants.Models;
using CIPP.Shared.DTOs;
using DispatchR.Abstractions.Send;
namespace CIPP.Api.Modules.Tenants.Queries;
public record GetTenantsQuery(
    string? Filter = null,
    int PageNumber = 1,
    int PageSize = 50,
    string? SortBy = null,
    bool SortDescending = false,
    bool NoCache = false
) : IRequest<GetTenantsQuery, Task<PagedResponse<Tenant>>>;
