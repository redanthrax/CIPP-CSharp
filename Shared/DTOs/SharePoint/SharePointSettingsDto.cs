namespace CIPP.Shared.DTOs.SharePoint;

public class SharePointSettingsDto {
    public string AdminUrl { get; set; } = string.Empty;
    public string? SharingCapability { get; set; }
    public bool? OneDriveForGuestsEnabled { get; set; }
    public bool? NotifyOwnersWhenItemsReshared { get; set; }
    public bool? PreventExternalUsersFromResharing { get; set; }
}
