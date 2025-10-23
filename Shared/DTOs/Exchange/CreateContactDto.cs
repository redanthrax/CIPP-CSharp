namespace CIPP.Shared.DTOs.Exchange;

public class CreateContactDto {
    public string DisplayName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string? GivenName { get; set; }
    public string? Surname { get; set; }
    public string? MiddleName { get; set; }
    public string? CompanyName { get; set; }
    public string? JobTitle { get; set; }
    public string? Department { get; set; }
    public string? OfficeLocation { get; set; }
    public string? MobilePhone { get; set; }
    public string? BusinessPhone { get; set; }
    public string? HomePhone { get; set; }
}
