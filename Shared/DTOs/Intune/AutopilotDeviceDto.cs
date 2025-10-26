namespace CIPP.Shared.DTOs.Intune;

public class AutopilotDeviceDto {
    public string Id { get; set; } = string.Empty;
    public string? SerialNumber { get; set; }
    public string? Model { get; set; }
    public string? Manufacturer { get; set; }
    public string? ProductKey { get; set; }
    public string? GroupTag { get; set; }
    public string? UserPrincipalName { get; set; }
    public string? AddressableUserName { get; set; }
    public string? AzureActiveDirectoryDeviceId { get; set; }
    public string? ManagedDeviceId { get; set; }
    public string? DisplayName { get; set; }
    public DateTime? EnrollmentState { get; set; }
    public DateTime? LastContactedDateTime { get; set; }
    public Guid TenantId { get; set; }
}
