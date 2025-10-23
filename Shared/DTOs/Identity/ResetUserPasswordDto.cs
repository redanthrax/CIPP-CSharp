using System.ComponentModel.DataAnnotations;

namespace CIPP.Shared.DTOs.Identity;

public class ResetUserPasswordDto {
    public string? Password { get; set; }
    public bool ForceChangePasswordNextSignIn { get; set; } = true;
    public bool AutoGeneratePassword { get; set; } = true;
    
    [Required]
    public string TenantId { get; set; } = string.Empty;
}