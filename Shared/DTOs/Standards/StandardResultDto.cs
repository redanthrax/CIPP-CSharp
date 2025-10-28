namespace CIPP.Shared.DTOs.Standards;

public class StandardResultDto {
    public Guid ExecutionId { get; set; }
    public Guid TenantId { get; set; }
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string? ErrorDetails { get; set; }
    public int? DurationMs { get; set; }
}
