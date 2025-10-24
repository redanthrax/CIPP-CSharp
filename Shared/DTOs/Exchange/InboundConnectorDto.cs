namespace CIPP.Shared.DTOs.Exchange;

public class InboundConnectorDto {
    public string Name { get; set; } = string.Empty;
    public bool Enabled { get; set; }
    public string ConnectorType { get; set; } = string.Empty;
    public string ConnectorSource { get; set; } = string.Empty;
    public List<string> SenderDomains { get; set; } = new();
    public List<string> SenderIPAddresses { get; set; } = new();
    public bool RequireTls { get; set; }
    public bool RestrictDomainsToCertificate { get; set; }
    public bool CloudServicesMailEnabled { get; set; }
}
