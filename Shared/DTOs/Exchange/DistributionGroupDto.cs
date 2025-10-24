namespace CIPP.Shared.DTOs.Exchange;

public class DistributionGroupDto {
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string PrimarySmtpAddress { get; set; } = string.Empty;
    public string Alias { get; set; } = string.Empty;
    public List<string> ManagedBy { get; set; } = new();
    public bool RequireSenderAuthenticationEnabled { get; set; }
    public int MemberCount { get; set; }
}
