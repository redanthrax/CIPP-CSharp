using System.ComponentModel.DataAnnotations;

namespace CIPP.Shared.DTOs.Exchange;

public class UpdateJournalRuleDto {
    [EmailAddress]
    public string? JournalEmailAddress { get; set; }
    
    public string? Scope { get; set; }
    
    public string? Recipient { get; set; }
    
    public bool? Enabled { get; set; }
    
    public bool? LawfulInterception { get; set; }
    
    public bool? FullReport { get; set; }
    
    public Guid TenantId { get; set; }
}
