using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Intune.Commands;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Intune;
using DispatchR;

namespace CIPP.Api.Modules.Intune.Endpoints;

public static class CreateIntunePolicy {
    public static void MapCreateIntunePolicy(this RouteGroupBuilder group) {
        group.MapPost("/", Handle)
            .WithName("CreateIntunePolicy")
            .WithSummary("Create Intune policy")
            .WithDescription("Creates a new Intune device management policy")
            .RequirePermission("Endpoint.MEM.ReadWrite", "Create Intune policy");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        CreateIntunePolicyDto request,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new CreateIntunePolicyCommand(tenantId, request);
            var result = await mediator.Send(command, cancellationToken);

            return Results.Ok(Response<IntunePolicyDto>.SuccessResult(result, "Policy created successfully"));
        } catch (NotSupportedException ex) {
            return Results.BadRequest(Response<IntunePolicyDto>.ErrorResult(ex.Message));
        } catch (Exception ex) {
            return Results.Problem(
                detail: ex.Message,
                statusCode: 500,
                title: "Error creating Intune policy"
            );
        }
    }
}
