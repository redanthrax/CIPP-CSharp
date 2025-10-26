using CIPP.Api.Modules.Tenants.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Tenants;
using DispatchR;
namespace CIPP.Api.Modules.Tenants.Endpoints;
public static class GetTenantById
{
    public static void MapGetTenantById(this RouteGroupBuilder group)
    {
        group.MapGet("/{tenantId:guid}", Handle)
            .WithName("GetTenantById")
            .WithSummary("Get tenant by TenantId")
            .WithDescription("Returns a specific tenant by its TenantId")
            .RequireAuthorization("CombinedAuth");
    }
    private static async Task<IResult> Handle(Guid tenantId, IMediator mediator)
    {
        try
        {
            var query = new GetTenantByIdQuery(tenantId);
            var tenant = await mediator.Send(query, CancellationToken.None);
            if (tenant == null)
            {
                return Results.NotFound(Response<TenantDto>.ErrorResult($"Tenant with TenantId {tenantId} not found"));
            }
            var tenantDto = new TenantDto(
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
