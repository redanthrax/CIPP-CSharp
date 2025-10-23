namespace CIPP.Shared.DTOs.Identity;

public class UpdateUserDto {
    public string? DisplayName { get; set; }
    public string? GivenName { get; set; }
    public string? Surname { get; set; }
    public string? Mail { get; set; }
    public string? JobTitle { get; set; }
    public string? Department { get; set; }
    public string? MobilePhone { get; set; }
    public string? BusinessPhone { get; set; }
    public string? OfficeLocation { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public string? StreetAddress { get; set; }
    public bool? AccountEnabled { get; set; }
    public List<string>? AssignedLicenses { get; set; }
    public List<string>? AssignedRoles { get; set; }
    public string? Manager { get; set; }
}