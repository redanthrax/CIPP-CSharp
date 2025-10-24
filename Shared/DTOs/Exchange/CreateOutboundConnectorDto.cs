namespace CIPP.Shared.DTOs.Exchange;

public class CreateOutboundConnectorDto {
    public string Name { get; set; } = string.Empty;
    public bool Enabled { get; set; } = true;
    public string ConnectorType { get; set; } = string.Empty;
    public List<string>? RecipientDomains { get; set; }
    public List<string>? SmartHosts { get; set; }
    public bool? UseMXRecord { get; set; }
    public bool? RequireTls { get; set; }
}
