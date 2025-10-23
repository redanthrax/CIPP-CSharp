namespace CIPP.Shared.DTOs.SharePoint;

public class TeamDetailsDto {
    public string Name { get; set; } = string.Empty;
    public TeamInfoDto? TeamInfo { get; set; }
    public List<TeamChannelDto> Channels { get; set; } = new();
    public List<TeamMemberDto> Members { get; set; } = new();
    public List<TeamMemberDto> Owners { get; set; } = new();
    public List<TeamAppDto> InstalledApps { get; set; } = new();
}
