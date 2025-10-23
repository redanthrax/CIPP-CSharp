namespace CIPP.Shared.DTOs.Exchange;

public class TransportRuleActionsDto {
    public List<string>? AddToRecipients { get; set; }
    public List<string>? BlindCopyTo { get; set; }
    public List<string>? CopyTo { get; set; }
    public List<string>? RedirectMessageTo { get; set; }
    public string? RejectMessageReasonText { get; set; }
    public string? DeleteMessage { get; set; }
    public string? Quarantine { get; set; }
    public string? SetSCL { get; set; }
    public string? PrependSubject { get; set; }
    public string? SetHeaderName { get; set; }
    public string? SetHeaderValue { get; set; }
    public string? RemoveHeader { get; set; }
    public string? ApplyClassification { get; set; }
    public string? ApplyHtmlDisclaimerText { get; set; }
    public string? ApplyHtmlDisclaimerLocation { get; set; }
    public string? ModerateMessageByUser { get; set; }
    public bool? StopRuleProcessing { get; set; }
}
