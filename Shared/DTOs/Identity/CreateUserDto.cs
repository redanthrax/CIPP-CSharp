using System.ComponentModel.DataAnnotations;

namespace CIPP.Shared.DTOs.Identity;

public class CreateUserDto {
    [Required]
    public string UserPrincipalName { get; set; } = string.Empty;
    
    [Required]
    public string DisplayName { get; set; } = string.Empty;
    
    public string GivenName { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Mail { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string MobilePhone { get; set; } = string.Empty;
    public string BusinessPhone { get; set; } = string.Empty;
    public string OfficeLocation { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string StreetAddress { get; set; } = string.Empty;
    public bool AccountEnabled { get; set; } = true;
    public string Password { get; set; } = string.Empty;
    public bool ForceChangePasswordNextSignIn { get; set; } = true;
    public List<string> AssignedLicenses { get; set; } = new();
    public List<string> AssignedRoles { get; set; } = new();
    public string? Manager { get; set; }
    
    [Required]
    public string TenantId { get; set; } = string.Empty;
}