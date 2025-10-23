namespace CIPP.Shared.DTOs.Security;

public class SecurityIncidentsResponseDto {
    public int NewIncidentsCount { get; set; }
    public int InProgressIncidentsCount { get; set; }
    public int HighSeverityCount { get; set; }
    public int MediumSeverityCount { get; set; }
    public int LowSeverityCount { get; set; }
    public List<SecurityIncidentDto> Incidents { get; set; } = new();
}
