namespace CIPP.Shared.DTOs.Identity;

public class BulkUserResultDto {
    public string ResultText { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string? CopyField { get; set; }
    public string? Username { get; set; }
}