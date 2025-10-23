namespace CIPP.Shared.DTOs.Security;

public class UpdateSecureScoreControlDto {
    public string State { get; set; } = string.Empty;
    public string? Comment { get; set; }
    public string? VendorInformation { get; set; }
}
