using System.ComponentModel.DataAnnotations;

namespace CIPP.Shared.DTOs.Identity;

public class BulkCreateUserDto {
    [Required]
    public string TenantFilter { get; set; } = string.Empty;
    
    [Required]
    public List<CreateUserDto> BulkUser { get; set; } = new();
    
    public List<string> Licenses { get; set; } = new();
    
    public string UsageLocation { get; set; } = "US";
}
