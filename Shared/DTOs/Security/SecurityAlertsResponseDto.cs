namespace CIPP.Shared.DTOs.Security;

public class SecurityAlertsResponseDto {
    public int NewAlertsCount { get; set; }
    public int InProgressAlertsCount { get; set; }
    public int SeverityHighAlertsCount { get; set; }
    public int SeverityMediumAlertsCount { get; set; }
    public int SeverityLowAlertsCount { get; set; }
    public int SeverityInformationalCount { get; set; }
    public List<SecurityAlertDto> Alerts { get; set; } = new();
}
