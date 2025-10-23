using System.ComponentModel.DataAnnotations;

namespace CIPP.Shared.DTOs.Identity;

public class CreateGroupDto {
    [Required]
    public string DisplayName { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    [Required]
    public string MailNickname { get; set; } = string.Empty;
    
    public List<string> GroupTypes { get; set; } = new();
    public bool SecurityEnabled { get; set; } = true;
    public bool MailEnabled { get; set; } = false;
    public string Visibility { get; set; } = "Private";
    public List<string> Owners { get; set; } = new();
    public List<string> Members { get; set; } = new();
    
    [Required]
    public string TenantId { get; set; } = string.Empty;
}