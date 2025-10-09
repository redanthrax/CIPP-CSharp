namespace CIPP.Shared.DTOs.Alerts;

public class AlertConditionDto {
    public AlertPropertyDto? Property { get; set; }
    public AlertOperatorDto? Operator { get; set; }
    public AlertInputDto? Input { get; set; }
}