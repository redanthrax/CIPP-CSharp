using System.ComponentModel.DataAnnotations;

namespace CIPP.Shared.DTOs.Exchange;

public class AddMailboxDelegateDto {
    [Required]
    [EmailAddress]
    public string DelegateUser { get; set; } = string.Empty;
    
    public List<string> Permissions { get; set; } = new();
    
    public bool SendOnBehalfOf { get; set; }
    
    public bool AutoMapping { get; set; } = true;
    
    public Guid TenantId { get; set; }
}
