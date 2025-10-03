using CIPP.Api.Modules.Authorization.Models;
using CIPP.Shared.DTOs;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Authorization.Queries;

public record GetApiKeysQuery() : IRequest<GetApiKeysQuery, Task<PagedResponse<ApiKey>>>;
