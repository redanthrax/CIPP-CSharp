namespace CIPP.Shared.DTOs.Intune;

public class IntuneAppDto {
    public string Id { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Publisher { get; set; }
    public DateTime? CreatedDateTime { get; set; }
    public DateTime? LastModifiedDateTime { get; set; }
    public string? AppType { get; set; }
    public bool IsAssigned { get; set; }
    public List<string>? AssignedTo { get; set; }
    public Guid TenantId { get; set; }
}
