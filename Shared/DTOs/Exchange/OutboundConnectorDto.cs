namespace CIPP.Shared.DTOs.Exchange;

public class OutboundConnectorDto {
    public string Name { get; set; } = string.Empty;
    public bool Enabled { get; set; }
    public string ConnectorType { get; set; } = string.Empty;
    public string ConnectorSource { get; set; } = string.Empty;
    public List<string> RecipientDomains { get; set; } = new();
    public List<string> SmartHosts { get; set; } = new();
    public bool UseMXRecord { get; set; }
    public bool RequireTls { get; set; }
    public bool CloudServicesMailEnabled { get; set; }
}
