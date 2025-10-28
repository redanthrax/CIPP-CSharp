namespace CIPP.Shared.DTOs.Exchange;

public class UpdateOwaMailboxPolicyDto {
    public bool? DirectFileAccessOnPublicComputersEnabled { get; set; }
    public bool? DirectFileAccessOnPrivateComputersEnabled { get; set; }
    public bool? WebReadyDocumentViewingOnPublicComputersEnabled { get; set; }
    public bool? WebReadyDocumentViewingOnPrivateComputersEnabled { get; set; }
    public bool? ForceWebReadyDocumentViewingFirstOnPublicComputers { get; set; }
    public bool? ForceWebReadyDocumentViewingFirstOnPrivateComputers { get; set; }
    public bool? WacViewingOnPublicComputersEnabled { get; set; }
    public bool? WacViewingOnPrivateComputersEnabled { get; set; }
    public bool? ClassicAttachmentsEnabled { get; set; }
    public bool? AllAddressListsEnabled { get; set; }
    public bool? CalendarEnabled { get; set; }
    public bool? ContactsEnabled { get; set; }
    public bool? JournalEnabled { get; set; }
    public bool? NotesEnabled { get; set; }
    public bool? RemindersAndNotificationsEnabled { get; set; }
    public bool? SatisfactionEnabled { get; set; }
    public bool? TextMessagingEnabled { get; set; }
    public bool? ThemeSelectionEnabled { get; set; }
    public bool? ChangePasswordEnabled { get; set; }
    public bool? UMIntegrationEnabled { get; set; }
    public bool? WSSAccessOnPublicComputersEnabled { get; set; }
    public bool? WSSAccessOnPrivateComputersEnabled { get; set; }
    public bool? UNCAccessOnPublicComputersEnabled { get; set; }
    public bool? UNCAccessOnPrivateComputersEnabled { get; set; }
    public bool? ActiveSyncIntegrationEnabled { get; set; }
    public string[]? AllowedFileTypes { get; set; }
    public string[]? AllowedMimeTypes { get; set; }
    public string[]? BlockedFileTypes { get; set; }
    public string[]? BlockedMimeTypes { get; set; }
}
