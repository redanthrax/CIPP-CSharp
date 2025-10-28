using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Standards.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Standards;
using DispatchR;

namespace CIPP.Api.Modules.Standards.Endpoints;

public static class DeployStandard {
    public static void MapDeployStandard(this RouteGroupBuilder group) {
        group.MapPost("/deploy", Handle)
            .WithName("DeployStandard")
            .WithSummary("Deploy standard")
            .WithDescription("Deploys a standard to one or more tenants")
            .RequirePermission("CIPP.Standards.Deploy", "Deploy standard");
    }

    private static async Task<IResult> Handle(
        DeployStandardDto deployDto,
        HttpContext context,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var executedBy = context.User.Identity?.Name ?? "system";
            var command = new DeployStandardCommand(deployDto, executedBy);
            var results = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<List<StandardResultDto>>.SuccessResult(results, "Standard deployment initiated"));
        } catch (InvalidOperationException ex) {
            return Results.BadRequest(Response<List<StandardResultDto>>.ErrorResult(ex.Message));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error deploying standard"
            );
        }
    }
}
