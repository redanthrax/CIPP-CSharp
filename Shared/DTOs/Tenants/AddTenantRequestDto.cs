namespace CIPP.Shared.DTOs.Tenants;

public class AddTenantRequestDto
{
    public string TenantName { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string AddressLine1 { get; set; } = string.Empty;
    public string? AddressLine2 { get; set; }
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    public AddTenantRequestDto() { }

    public AddTenantRequestDto(string tenantName, string companyName, string addressLine1, string? addressLine2,
        string city, string state, string postalCode, string country, string firstName, string lastName,
        string email, string phoneNumber)
    {
        TenantName = tenantName;
        CompanyName = companyName;
        AddressLine1 = addressLine1;
        AddressLine2 = addressLine2;
        City = city;
        State = state;
        PostalCode = postalCode;
        Country = country;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
    }
}
