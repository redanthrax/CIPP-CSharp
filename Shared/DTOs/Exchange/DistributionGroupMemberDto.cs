namespace CIPP.Shared.DTOs.Exchange;

public class DistributionGroupMemberDto {
    public string Name { get; set; } = string.Empty;
    public string PrimarySmtpAddress { get; set; } = string.Empty;
    public string RecipientType { get; set; } = string.Empty;
}
