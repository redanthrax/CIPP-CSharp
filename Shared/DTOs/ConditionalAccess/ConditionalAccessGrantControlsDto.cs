namespace CIPP.Shared.DTOs.ConditionalAccess;

public class ConditionalAccessGrantControlsDto {
    public string Operator { get; set; } = string.Empty;
    public List<string>? BuiltInControls { get; set; }
    public List<string>? CustomAuthenticationFactors { get; set; }
    public List<string>? TermsOfUse { get; set; }
}
