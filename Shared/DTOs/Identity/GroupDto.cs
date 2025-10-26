namespace CIPP.Shared.DTOs.Identity;

public class GroupDto {
    public string Id { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Mail { get; set; } = string.Empty;
    public string MailNickname { get; set; } = string.Empty;
    public List<string> GroupTypes { get; set; } = new();
    public bool SecurityEnabled { get; set; }
    public bool MailEnabled { get; set; }
    public string Visibility { get; set; } = string.Empty;
    public DateTime? CreatedDateTime { get; set; }
    public int MemberCount { get; set; }
    public List<string> Owners { get; set; } = new();
    public List<string> Members { get; set; } = new();
    public Guid? TenantId { get; set; }
}