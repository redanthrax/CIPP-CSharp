namespace CIPP.Shared.DTOs.Identity;

public class UserMfaStatusDto {
    public bool Enabled { get; set; }
    public bool Enforced { get; set; }
    public List<string> Methods { get; set; } = new();
    public string DefaultMethod { get; set; } = string.Empty;
}