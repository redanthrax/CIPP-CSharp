namespace CIPP.Shared.DTOs.Exchange;

public class SafeAttachmentsPolicyDto {
    public string Name { get; set; } = string.Empty;
    public bool Enable { get; set; }
    public string Action { get; set; } = string.Empty;
    public string QuarantineTag { get; set; } = string.Empty;
    public bool Redirect { get; set; }
    public string? RedirectAddress { get; set; }
}
