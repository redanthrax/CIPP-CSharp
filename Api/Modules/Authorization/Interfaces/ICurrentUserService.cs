namespace CIPP.Api.Modules.Authorization.Interfaces;

public interface ICurrentUserService {
    Guid? GetCurrentUserId();

    string? GetCurrentUserEmail();

    string? GetCurrentUserDisplayName();

    string? GetCurrentUserAzureAdObjectId();

    bool IsAuthenticated();
}
