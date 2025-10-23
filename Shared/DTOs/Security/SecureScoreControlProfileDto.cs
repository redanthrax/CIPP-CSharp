namespace CIPP.Shared.DTOs.Security;

public class SecureScoreControlProfileDto {
    public string Id { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? State { get; set; }
    public string? AzureTenantId { get; set; }
    public string? ControlCategory { get; set; }
    public int? MaxScore { get; set; }
    public string? Rank { get; set; }
    public List<string>? Remediation { get; set; }
    public string? RemediationImpact { get; set; }
    public string? UserImpact { get; set; }
    public string? ImplementationCost { get; set; }
    public bool? Deprecated { get; set; }
}
