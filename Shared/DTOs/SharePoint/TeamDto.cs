namespace CIPP.Shared.DTOs.SharePoint;

public class TeamDto {
    public string Id { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Visibility { get; set; }
    public string? MailNickname { get; set; }
}
