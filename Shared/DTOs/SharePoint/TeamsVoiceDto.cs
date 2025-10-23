namespace CIPP.Shared.DTOs.SharePoint;

public class TeamsVoiceDto {
    public string TelephoneNumber { get; set; } = string.Empty;
    public string? NumberType { get; set; }
    public string? TargetId { get; set; }
    public string? AssignedTo { get; set; }
    public string? AcquisitionDate { get; set; }
    public string? Location { get; set; }
}
