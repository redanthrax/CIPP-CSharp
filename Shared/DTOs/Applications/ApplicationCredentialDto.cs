namespace CIPP.Shared.DTOs.Applications;

public class ApplicationCredentialDto {
    public string? KeyId { get; set; }
    public string? DisplayName { get; set; }
    public DateTime? StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
    public string? Hint { get; set; }
    public string? Type { get; set; }
    public string? Usage { get; set; }
}
