using CIPP.Api.Modules.Identity.Commands;
using CIPP.Api.Modules.MsGraph.Interfaces;
using DispatchR.Abstractions.Send;
using Microsoft.Graph.Beta.Models;

namespace CIPP.Api.Modules.Identity.Handlers;

public class SetUserSignInStateCommandHandler : IRequestHandler<SetUserSignInStateCommand, Task<string>> {
    private readonly IMicrosoftGraphService _graphService;
    private readonly ILogger<SetUserSignInStateCommandHandler> _logger;

    public SetUserSignInStateCommandHandler(
        IMicrosoftGraphService graphService,
        ILogger<SetUserSignInStateCommandHandler> logger) {
        _graphService = graphService;
        _logger = logger;
    }

    public async Task<string> Handle(SetUserSignInStateCommand request, CancellationToken cancellationToken) {
        _logger.LogInformation("Setting sign-in state for user {UserId} to {State} in tenant {TenantId}",
            request.UserSignInStateData.ID, request.UserSignInStateData.Enable, request.UserSignInStateData.TenantFilter);

        try {
            var graphClient = await _graphService.GetGraphServiceClientAsync(request.UserSignInStateData.TenantFilter);
            
            var user = new User {
                AccountEnabled = request.UserSignInStateData.Enable
            };

            await graphClient.Users[request.UserSignInStateData.ID].PatchAsync(user, null, cancellationToken);

            var action = request.UserSignInStateData.Enable ? "enabled" : "disabled";
            return $"Successfully {action} sign-in for user {request.UserSignInStateData.ID}";

        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to set sign-in state for user {UserId}", request.UserSignInStateData.ID);
            throw new InvalidOperationException($"Failed to set sign-in state: {ex.Message}", ex);
        }
    }
}