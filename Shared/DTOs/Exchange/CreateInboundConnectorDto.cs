namespace CIPP.Shared.DTOs.Exchange;

public class CreateInboundConnectorDto {
    public string Name { get; set; } = string.Empty;
    public bool Enabled { get; set; } = true;
    public string ConnectorType { get; set; } = string.Empty;
    public List<string>? SenderDomains { get; set; }
    public List<string>? SenderIPAddresses { get; set; }
    public bool? RequireTls { get; set; }
}
