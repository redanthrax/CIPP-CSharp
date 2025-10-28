using System.ComponentModel.DataAnnotations;

namespace CIPP.Shared.DTOs.Exchange;

public class CreateJournalRuleDto {
    [Required]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    public string JournalEmailAddress { get; set; } = string.Empty;
    
    public string Scope { get; set; } = "Global";
    
    public string? Recipient { get; set; }
    
    public bool Enabled { get; set; } = true;
    
    public bool LawfulInterception { get; set; } = false;
    
    public bool FullReport { get; set; } = true;
    
    public Guid TenantId { get; set; }
}
