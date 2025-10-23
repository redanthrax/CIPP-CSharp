namespace CIPP.Shared.DTOs.SharePoint;

public class AssignTeamsPhoneNumberDto {
    public string Identity { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? PhoneNumberType { get; set; }
    public bool LocationOnly { get; set; }
    public string? LocationId { get; set; }
}
