namespace CIPP.Shared.DTOs.SharePoint;

public class TeamsActivityDto {
    public string UPN { get; set; } = string.Empty;
    public string? LastActive { get; set; }
    public int TeamsChat { get; set; }
    public int CallCount { get; set; }
    public int MeetingCount { get; set; }
}
