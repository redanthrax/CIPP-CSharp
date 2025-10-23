using System.ComponentModel.DataAnnotations;

namespace CIPP.Shared.DTOs.Identity;

public class SetUserSignInStateDto {
    [Required]
    public string TenantFilter { get; set; } = string.Empty;
    
    [Required]
    public string ID { get; set; } = string.Empty;
    
    [Required]
    public bool Enable { get; set; }
}