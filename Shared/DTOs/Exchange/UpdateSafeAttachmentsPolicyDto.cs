namespace CIPP.Shared.DTOs.Exchange;

public class UpdateSafeAttachmentsPolicyDto {
    public bool? Enable { get; set; }
    public string? Action { get; set; }
    public string? QuarantineTag { get; set; }
    public bool? Redirect { get; set; }
    public string? RedirectAddress { get; set; }
}
