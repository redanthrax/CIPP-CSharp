namespace CIPP.Shared.DTOs.Alerts;

public class AlertStatisticsDto {
    public int TotalAlerts { get; set; }
    public int ActiveRules { get; set; }
    public int TriggeredToday { get; set; }
    public int TriggeredThisWeek { get; set; }
    public int TriggeredThisMonth { get; set; }
    public Dictionary<string, int> AlertsByType { get; set; } = new();
    public Dictionary<string, int> AlertsByTenant { get; set; } = new();
    public List<RecentAlertDto> RecentAlerts { get; set; } = new();
}
