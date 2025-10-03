using CIPP.Api.Modules.Authorization.Commands;
using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Authorization.Models;
using CIPP.Api.Modules.Authorization.Interfaces;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.ApiKeys;
using DispatchR;

namespace CIPP.Api.Modules.Authorization.Endpoints;

public static class CreateApiKey {
    public static void MapCreateApiKey(this RouteGroupBuilder group) {
        group.MapPost("/apikeys", Handle)
            .WithName("CreateApiKey")
            .WithSummary("Create a new API key")
            .WithDescription("Creates a new API key with optional role assignments for granular access control")
            .RequirePermission("ApiKey.Create", "Create new API keys in the system");
    }

    private static async Task<IResult> Handle(
        CreateApiKeyDto createApiKeyDto, 
        IMediator mediator,
        ICurrentUserService currentUserService) {
        try {
            var createdBy = currentUserService.GetCurrentUserEmail() ?? "System";
            
            var command = new CreateApiKeyCommand(
                createApiKeyDto.Name,
                createApiKeyDto.Description,
                createdBy,
                createApiKeyDto.ExpiresAt,
                createApiKeyDto.RoleIds
            );

            (ApiKey entity, string generatedKey) = await mediator.Send(command, CancellationToken.None);

            var responseDto = new CreateApiKeyResponseDto(
                entity.Id,
                entity.Name,
                generatedKey,
                entity.Description,
                entity.CreatedAt,
                entity.CreatedBy,
                entity.ExpiresAt,
                entity.ApiKeyRoles.Select(akr => new ApiKeyRoleDto(
                    akr.Role.Id,
                    akr.Role.Name,
                    akr.Role.Description,
                    akr.CreatedAt,
                    akr.CreatedBy,
                    akr.ExpiresAt
                )).ToList()
            );

            return Results.Created($"/api/authorization/apikeys/{entity.Id}", Response<CreateApiKeyResponseDto>.SuccessResult(responseDto));
        }
        catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error creating API key"
            );
        }
    }
}
