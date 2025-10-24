namespace CIPP.Shared.DTOs.Exchange;

public class UpdateInboxRuleDto {
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? Enabled { get; set; }
    public int? Priority { get; set; }
    public List<string>? BodyContainsWords { get; set; }
    public List<string>? SubjectContainsWords { get; set; }
    public List<string>? From { get; set; }
    public string? MoveToFolder { get; set; }
    public string? CopyToFolder { get; set; }
    public bool? DeleteMessage { get; set; }
    public bool? MarkAsRead { get; set; }
    public bool? StopProcessingRules { get; set; }
    public List<string>? ForwardTo { get; set; }
    public List<string>? RedirectTo { get; set; }
}
