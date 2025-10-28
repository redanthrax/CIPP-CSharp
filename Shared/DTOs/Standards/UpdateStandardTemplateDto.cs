namespace CIPP.Shared.DTOs.Standards;

public class UpdateStandardTemplateDto {
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Configuration { get; set; }
    public bool? IsEnabled { get; set; }
}
