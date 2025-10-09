using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Tenants.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Tenants;
using DispatchR;

namespace CIPP.Api.Modules.Tenants.Endpoints;

public static class GetTenantGroups {
    public static void MapGetTenantGroups(this RouteGroupBuilder group) {
        group.MapGet("/groups", Handle)
            .WithName("GetTenantGroups")
            .WithSummary("Get tenant groups")
            .WithDescription("Returns a paginated list of tenant groups with their members. " +
                            "Parameters: pageNumber (default: 1), pageSize (default: 50), groupId (optional filter)")
            .RequirePermission("Tenant.Groups.Read", "View tenant groups and their memberships");
    }

    private static async Task<IResult> Handle(
        IMediator mediator,
        int pageNumber = 1,
        int pageSize = 50,
        Guid? groupId = null,
        CancellationToken cancellationToken = default) {
        try {
            var query = new GetTenantGroupsQuery(groupId);
            var tenantGroups = await mediator.Send(query, cancellationToken);

            var tenantGroupDtos = tenantGroups.Select(g => new TenantGroupDto(
                g.Id,
                g.Name,
                g.Description,
                g.CreatedAt,
                g.CreatedBy,
                g.UpdatedAt,
                g.UpdatedBy,
                g.Memberships.Select(m => m.TenantId).ToList()
            )).ToList();

            // Apply pagination
            var paginatedItems = tenantGroupDtos
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var pagedResponse = new PagedResponse<TenantGroupDto> {
                Items = paginatedItems,
                TotalCount = tenantGroupDtos.Count,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return Results.Ok(Response<PagedResponse<TenantGroupDto>>.SuccessResult(pagedResponse));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving tenant groups"
            );
        }
    }
}