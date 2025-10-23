namespace CIPP.Shared.DTOs.SharePoint;

public class TeamMemberDto {
    public string Id { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public List<string> Roles { get; set; } = new();
}
