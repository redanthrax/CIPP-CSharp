namespace CIPP.Shared.DTOs.Exchange;

public class ContactDetailsDto : ContactDto {
    public string? MiddleName { get; set; }
    public string? NickName { get; set; }
    public string? Title { get; set; }
    public List<string> EmailAddresses { get; set; } = new();
    public string? HomePhone { get; set; }
    public string? BusinessPhone2 { get; set; }
    public string? HomeAddress { get; set; }
    public string? BusinessAddress { get; set; }
    public string? OtherAddress { get; set; }
    public string? Manager { get; set; }
    public string? AssistantName { get; set; }
    public DateTime? Birthday { get; set; }
    public string? Profession { get; set; }
    public string? SpouseName { get; set; }
    public string? PersonalNotes { get; set; }
    public List<string> Categories { get; set; } = new();
}
