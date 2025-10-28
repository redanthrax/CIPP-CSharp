using System.ComponentModel.DataAnnotations;

namespace CIPP.Shared.DTOs.Exchange;

public class RemoveMailboxDelegateDto {
    [Required]
    [EmailAddress]
    public string DelegateUser { get; set; } = string.Empty;
    
    public List<string> Permissions { get; set; } = new();
    
    public bool RemoveSendOnBehalfOf { get; set; }
    
    public Guid TenantId { get; set; }
}
