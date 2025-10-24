using CIPP.Api.Modules.Authorization.Extensions;
using CIPP.Api.Modules.Exchange.Commands.DistributionGroups;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;
using DispatchR;

namespace CIPP.Api.Modules.Exchange.Endpoints.DistributionGroups;

public static class CreateDistributionGroup {
    public static void MapCreateDistributionGroup(this RouteGroupBuilder group) {
        group.MapPost("/", Handle)
            .WithName("CreateDistributionGroup")
            .WithSummary("Create distribution group")
            .WithDescription("Creates a new distribution group")
            .RequirePermission("Exchange.DistributionGroup.Write", "Create distribution groups");
    }

    private static async Task<IResult> Handle(
        string tenantId,
        CreateDistributionGroupDto dto,
        IMediator mediator,
        CancellationToken cancellationToken = default) {
        try {
            var command = new CreateDistributionGroupCommand(tenantId, dto);
            await mediator.Send(command, cancellationToken);
            return Results.Ok(Response<object>.SuccessResult(null, "Distribution group created successfully"));
        } catch (Exception ex) {
            return Results.Problem(detail: ex.Message, statusCode: 500, title: "Error creating distribution group");
        }
    }
}
