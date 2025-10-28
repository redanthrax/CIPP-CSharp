namespace CIPP.Shared.DTOs.Exchange;

public class SharedMailboxDto {
    public string Identity { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string PrimarySmtpAddress { get; set; } = string.Empty;
    public string Alias { get; set; } = string.Empty;
    public string UserPrincipalName { get; set; } = string.Empty;
    public string[] EmailAddresses { get; set; } = Array.Empty<string>();
    public bool IsShared { get; set; }
    public long ItemCount { get; set; }
    public long TotalItemSize { get; set; }
    public string[] Members { get; set; } = Array.Empty<string>();
    public bool HiddenFromAddressListsEnabled { get; set; }
}
