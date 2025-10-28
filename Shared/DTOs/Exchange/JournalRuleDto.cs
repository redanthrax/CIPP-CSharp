namespace CIPP.Shared.DTOs.Exchange;

public class JournalRuleDto {
    public string Name { get; set; } = string.Empty;
    public bool Enabled { get; set; }
    public string? JournalEmailAddress { get; set; }
    public string? Scope { get; set; }
    public string? Recipient { get; set; }
    public bool? LawfulInterception { get; set; }
    public bool? FullReport { get; set; }
    public Guid TenantId { get; set; }
}
