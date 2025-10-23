using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.ConditionalAccess.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.ConditionalAccess;
using DispatchR;

namespace CIPP.Api.Modules.ConditionalAccess.Endpoints;

public static class DeployConditionalAccessTemplate {
    public static void MapDeployConditionalAccessTemplate(this RouteGroupBuilder group) {
        group.MapPost("/{id}/deploy", Handle)
            .WithName("DeployConditionalAccessTemplate")
            .WithSummary("Deploy CA template to tenant")
            .WithDescription("Deploys a conditional access template as a policy to the specified tenant")
            .RequirePermission("Tenant.ConditionalAccess.ReadWrite", "Deploy CA template");
    }

    private static async Task<IResult> Handle(
        Guid id,
        DeployConditionalAccessTemplateDto deployDto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            deployDto.TemplateId = id;
            var command = new DeployConditionalAccessTemplateCommand(deployDto);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<ConditionalAccessPolicyDto>.SuccessResult(result, "Template deployed successfully"));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error deploying template"
            );
        }
    }
}
