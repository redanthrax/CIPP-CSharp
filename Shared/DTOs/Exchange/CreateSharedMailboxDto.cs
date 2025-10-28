namespace CIPP.Shared.DTOs.Exchange;

public class CreateSharedMailboxDto {
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Alias { get; set; } = string.Empty;
    public string PrimarySmtpAddress { get; set; } = string.Empty;
    public bool HiddenFromAddressListsEnabled { get; set; }
}
