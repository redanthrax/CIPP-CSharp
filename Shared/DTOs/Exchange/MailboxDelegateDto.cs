namespace CIPP.Shared.DTOs.Exchange;

public class MailboxDelegateDto {
    public string MailboxId { get; set; } = string.Empty;
    public string DelegateUser { get; set; } = string.Empty;
    public List<string> Permissions { get; set; } = new();
    public bool SendOnBehalfOf { get; set; }
    public Guid TenantId { get; set; }
}
