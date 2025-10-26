namespace CIPP.Shared.DTOs.Identity;

public class DeviceDto {
    public string Id { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string DeviceId { get; set; } = string.Empty;
    public string OperatingSystem { get; set; } = string.Empty;
    public string OperatingSystemVersion { get; set; } = string.Empty;
    public bool IsCompliant { get; set; }
    public bool IsManaged { get; set; }
    public string TrustType { get; set; } = string.Empty;
    public DateTime? ApproximateLastSignInDateTime { get; set; }
    public DateTime? RegistrationDateTime { get; set; }
    public bool AccountEnabled { get; set; }
    public string? Manufacturer { get; set; }
    public string? Model { get; set; }
    public List<string> PhysicalIds { get; set; } = new();
    public Guid? TenantId { get; set; }
}