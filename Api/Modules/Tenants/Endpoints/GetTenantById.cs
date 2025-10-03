using CIPP.Api.Modules.Tenants.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Tenants;
using DispatchR;
namespace CIPP.Api.Modules.Tenants.Endpoints;
public static class GetTenantById
{
    public static void MapGetTenantById(this RouteGroupBuilder group)
    {
        group.MapGet("/{id:guid}", Handle)
            .WithName("GetTenantById")
            .WithSummary("Get tenant by ID")
            .WithDescription("Returns a specific tenant by its ID")
            .RequireAuthorization("CombinedAuth");
    }
    private static async Task<IResult> Handle(Guid id, IMediator mediator)
    {
        try
        {
            var query = new GetTenantByIdQuery(id);
            var tenant = await mediator.Send(query, CancellationToken.None);
            if (tenant == null)
            {
                return Results.NotFound($"Tenant with ID {id} not found");
            }
            var tenantDto = new TenantDto(
                tenant.Id,
                tenant.TenantId,
                tenant.DisplayName,
                tenant.DefaultDomainName,
                tenant.Status,
                tenant.CreatedAt,
                tenant.CreatedBy,
                tenant.Metadata
            );
            return Results.Ok(Response<TenantDto>.SuccessResult(tenantDto));
        }
        catch (Exception ex)
        {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving tenant"
            );
        }
    }
}
