using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.ConditionalAccess.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.ConditionalAccess;
using DispatchR;

namespace CIPP.Api.Modules.ConditionalAccess.Endpoints;

public static class CreateConditionalAccessPolicy {
    public static void MapCreateConditionalAccessPolicy(this RouteGroupBuilder group) {
        group.MapPost("/policies", Handle)
            .WithName("CreateConditionalAccessPolicy")
            .WithSummary("Create a conditional access policy")
            .WithDescription("Creates a new conditional access policy for the specified tenant")
            .RequirePermission("ConditionalAccess.Policy.Write", "Create conditional access policies");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        CreateConditionalAccessPolicyDto policy,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            policy.TenantId = tenantId;
            var command = new CreateConditionalAccessPolicyCommand(policy);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<ConditionalAccessPolicyDto>.SuccessResult(result, "Policy created successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error creating conditional access policy"
            );
        }
    }
}
