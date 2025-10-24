namespace CIPP.Shared.DTOs.Exchange;

public class InboxRuleDto {
    public string Identity { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool Enabled { get; set; }
    public int Priority { get; set; }
    public bool IsValid { get; set; }
    public string? RuleIdentity { get; set; }
    public List<string> BodyContainsWords { get; set; } = new();
    public List<string> SubjectContainsWords { get; set; } = new();
    public List<string> From { get; set; } = new();
    public List<string> MoveToFolder { get; set; } = new();
    public List<string> CopyToFolder { get; set; } = new();
    public bool DeleteMessage { get; set; }
    public bool MarkAsRead { get; set; }
    public bool StopProcessingRules { get; set; }
    public List<string> ForwardTo { get; set; } = new();
    public List<string> RedirectTo { get; set; } = new();
}
