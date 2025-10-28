namespace CIPP.Api.Modules.Standards.Models;

public class SharePointSettingsOverride {
    public bool? DisableExternalSharing { get; set; }
    public bool? RequireMFAForExternalSharing { get; set; }
    public int? StorageQuotaMB { get; set; }
    public Dictionary<string, object>? AdditionalSettings { get; set; }
}
