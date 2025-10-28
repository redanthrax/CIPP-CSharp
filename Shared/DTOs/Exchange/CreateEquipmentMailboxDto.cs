namespace CIPP.Shared.DTOs.Exchange;

public class CreateEquipmentMailboxDto {
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Alias { get; set; } = string.Empty;
    public string ResourceCapacity { get; set; } = string.Empty;
}
