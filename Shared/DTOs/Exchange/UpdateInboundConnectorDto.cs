namespace CIPP.Shared.DTOs.Exchange;

public class UpdateInboundConnectorDto {
    public bool? Enabled { get; set; }
    public List<string>? SenderDomains { get; set; }
    public List<string>? SenderIPAddresses { get; set; }
    public bool? RequireTls { get; set; }
}
