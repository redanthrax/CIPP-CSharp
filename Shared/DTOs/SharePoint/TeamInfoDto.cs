namespace CIPP.Shared.DTOs.SharePoint;

public class TeamInfoDto {
    public string Id { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Visibility { get; set; }
    public bool IsArchived { get; set; }
    public string? WebUrl { get; set; }
}
