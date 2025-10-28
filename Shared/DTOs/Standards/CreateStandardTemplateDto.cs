namespace CIPP.Shared.DTOs.Standards;

public class CreateStandardTemplateDto {
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Configuration { get; set; } = "{}";
    public bool IsEnabled { get; set; } = true;
    public bool IsGlobal { get; set; }
}
