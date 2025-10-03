using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Authorization.Queries;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.ApiKeys;
using DispatchR;

namespace CIPP.Api.Modules.Authorization.Endpoints;

public static class GetApiKeys {
    public static void MapGetApiKeys(this RouteGroupBuilder group) {
        group.MapGet("/apikeys", Handle)
            .WithName("GetApiKeys")
            .WithSummary("Get all API keys")
            .WithDescription("Retrieves all active API keys with their role assignments")
            .RequirePermission("ApiKey.Read", "Read API keys in the system");
    }

    private static async Task<IResult> Handle(IMediator mediator, CancellationToken cancellationToken) {
        try {
            var query = new GetApiKeysQuery();
            var result = await mediator.Send(query, cancellationToken);

            var apiKeyDtos = result.Items.Select(apiKey => new ApiKeyDto(
                apiKey.Id,
                apiKey.Name,
                apiKey.Description,
                apiKey.CreatedAt,
                apiKey.CreatedBy,
                apiKey.ExpiresAt,
                apiKey.LastUsedAt,
                apiKey.UsageCount,
                apiKey.ApiKeyRoles.Select(akr => new ApiKeyRoleDto(
                    akr.Role.Id,
                    akr.Role.Name,
                    akr.Role.Description,
                    akr.CreatedAt,
                    akr.CreatedBy,
                    akr.ExpiresAt
                )).ToList()
            )).ToList();

            var response = new PagedResponse<ApiKeyDto> {
                Items = apiKeyDtos,
                TotalCount = result.TotalCount,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize
            };

            return Results.Ok(Response<PagedResponse<ApiKeyDto>>.SuccessResult(response));
        }
        catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error retrieving API keys"
            );
        }
    }
}
