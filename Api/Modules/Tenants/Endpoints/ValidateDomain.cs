using CIPP.Api.Modules.Tenants.Queries;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Tenants;
using DispatchR;

namespace CIPP.Api.Modules.Tenants.Endpoints;

public static class ValidateDomain
{
    public static void MapValidateDomain(this RouteGroupBuilder group)
    {
        group.MapGet("/validate-domain", Handle)
            .WithName("ValidateDomain")
            .WithSummary("Validate tenant domain availability")
            .WithDescription("Validates if a tenant domain name is available for use")
            .RequirePermission("Tenant.Create", "Validate domain names for tenant creation");
    }

    private static async Task<IResult> Handle(
        IMediator mediator,
        string tenantName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new ValidateDomainQuery(tenantName);
            var result = await mediator.Send(query, cancellationToken);
            return Results.Ok(Response<ValidateDomainResponseDto>.SuccessResult(result));
        }
        catch (Exception ex)
        {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error validating domain"
            );
        }
    }
}