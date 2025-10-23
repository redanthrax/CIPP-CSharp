using System.ComponentModel.DataAnnotations;

namespace CIPP.Shared.DTOs.Identity;

public class AddGroupMemberDto {
    [Required]
    public string UserId { get; set; } = string.Empty;
    
    public bool AsOwner { get; set; } = false;
    
    [Required]
    public string TenantId { get; set; } = string.Empty;
}