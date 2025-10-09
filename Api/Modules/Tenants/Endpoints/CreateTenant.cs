using CIPP.Api.Modules.Tenants.Commands;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Authorization.Interfaces;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Tenants;
using DispatchR;
namespace CIPP.Api.Modules.Tenants.Endpoints;
public static class CreateTenant
{
    public static void MapCreateTenant(this RouteGroupBuilder group)
    {
        group.MapPost("/", Handle)
            .WithName("CreateTenant")
            .WithSummary("Create a new tenant")
            .WithDescription("Creates a new tenant in the system")
            .RequirePermission("Tenant.Create", "Create new tenants in the system");
    }
    private static async Task<IResult> Handle(
        CreateTenantDto createTenantDto, 
        IMediator mediator,
        ICurrentUserService currentUserService,
        CancellationToken cancellationToken)
    {
        try
        {
            var currentUserId = currentUserService.GetCurrentUserId() ?? Guid.Empty;
            var command = new CreateTenantCommand(
                createTenantDto.TenantId,
                createTenantDto.DisplayName,
                createTenantDto.DefaultDomainName,
                createTenantDto.Status,
                currentUserId,
                createTenantDto.Metadata
            );
            var tenant = await mediator.Send(command, cancellationToken);
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
            return Results.Created($"/api/tenants/{tenant.Id}", Response<TenantDto>.SuccessResult(tenantDto));
        }
        catch (Exception ex)
        {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error creating tenant"
            );
        }
    }
}
