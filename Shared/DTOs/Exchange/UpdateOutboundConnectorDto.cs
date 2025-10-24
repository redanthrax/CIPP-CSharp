namespace CIPP.Shared.DTOs.Exchange;

public class UpdateOutboundConnectorDto {
    public bool? Enabled { get; set; }
    public List<string>? RecipientDomains { get; set; }
    public List<string>? SmartHosts { get; set; }
    public bool? UseMXRecord { get; set; }
    public bool? RequireTls { get; set; }
}
