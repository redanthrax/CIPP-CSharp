using System.ComponentModel.DataAnnotations;

namespace CIPP.Shared.DTOs.Identity;

public class AssignRoleDto {
    [Required]
    public string UserId { get; set; } = string.Empty;
    
    [Required]
    public string RoleId { get; set; } = string.Empty;
    
    [Required]
    public Guid TenantId { get; set; }
}