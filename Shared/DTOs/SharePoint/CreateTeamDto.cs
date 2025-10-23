namespace CIPP.Shared.DTOs.SharePoint;

public class CreateTeamDto {
    public string DisplayName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Visibility { get; set; } = "Private";
    public List<string> Owners { get; set; } = new();
}
