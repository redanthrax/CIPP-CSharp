using CIPP.Api.Modules.Tenants.Queries;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Tenants;
using DispatchR;
namespace CIPP.Api.Modules.Tenants.Endpoints;
public static class GetTenants
{
    public static void MapGetTenants(this RouteGroupBuilder group)
    {
        group.MapGet("/", Handle)
            .WithName("GetTenants")
            .WithSummary("Get all tenants")
            .WithDescription("Returns a paginated list of tenants with optional filtering and sorting. " +
                            "Parameters: filter, pageNumber (default: 1), pageSize (default: 50), sortBy, " +
                            "sortDescending (default: false), noCache (default: false) - forces sync from MS Graph. " +
                            "Automatically syncs from Graph if no tenants exist in database.")
            .RequirePermission("Tenant.Read", "View and list all tenants");
    }
    private static async Task<IResult> Handle(
        IMediator mediator,
        string? filter = null,
        int pageNumber = 1,
        int pageSize = 50,
        string? sortBy = null,
        bool sortDescending = false,
        bool noCache = false,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new GetTenantsQuery(
                Filter: filter,
                PageNumber: pageNumber,
                PageSize: pageSize,
                SortBy: sortBy,
                SortDescending: sortDescending,
                NoCache: noCache
            );
            var pagedTenants = await mediator.Send(query, cancellationToken);
            var tenantDtos = pagedTenants.Items.Select(t => new TenantDto(
                t.Id,
                t.TenantId,
                t.DisplayName,
                t.DefaultDomainName,
                t.Status,
                t.CreatedAt,
                t.CreatedBy,
                t.Metadata
            )).ToList();
            var pagedTenantDtos = new PagedResponse<TenantDto>
            {
                Items = tenantDtos,
                TotalCount = pagedTenants.TotalCount,
                PageNumber = pagedTenants.PageNumber,
                PageSize = pagedTenants.PageSize
            };
            return Results.Ok(Response<PagedResponse<TenantDto>>.SuccessResult(pagedTenantDtos));
        }
        catch (Exception ex)
        {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving tenants"
            );
        }
    }
}
