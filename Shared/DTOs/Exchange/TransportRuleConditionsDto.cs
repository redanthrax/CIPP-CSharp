namespace CIPP.Shared.DTOs.Exchange;

public class TransportRuleConditionsDto {
    public List<string>? From { get; set; }
    public List<string>? FromMemberOf { get; set; }
    public List<string>? SentTo { get; set; }
    public List<string>? SentToMemberOf { get; set; }
    public List<string>? SubjectContainsWords { get; set; }
    public List<string>? SubjectMatchesPatterns { get; set; }
    public List<string>? FromAddressContainsWords { get; set; }
    public List<string>? RecipientAddressContainsWords { get; set; }
    public List<string>? AttachmentNameMatchesPatterns { get; set; }
    public List<string>? HeaderContainsWords { get; set; }
    public string? MessageSizeOver { get; set; }
    public string? SCLOver { get; set; }
    public bool? HasAttachment { get; set; }
    public List<string>? AnyOfToHeader { get; set; }
    public List<string>? AnyOfCcHeader { get; set; }
    public string? SenderDomainIs { get; set; }
    public string? RecipientDomainIs { get; set; }
}
