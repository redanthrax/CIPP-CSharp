namespace CIPP.Shared.DTOs.Intune;

public class IntunePolicyDto {
    public string Id { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? LastModifiedDateTime { get; set; }
    public string? PolicyTypeName { get; set; }
    public string? URLName { get; set; }
    public string? PolicyAssignment { get; set; }
    public string? PolicyExclude { get; set; }
    public List<string>? RoleScopeTagIds { get; set; }
    public string TenantId { get; set; } = string.Empty;
}
