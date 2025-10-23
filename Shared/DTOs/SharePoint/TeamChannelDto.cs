namespace CIPP.Shared.DTOs.SharePoint;

public class TeamChannelDto {
    public string Id { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? MembershipType { get; set; }
    public string? WebUrl { get; set; }
}
