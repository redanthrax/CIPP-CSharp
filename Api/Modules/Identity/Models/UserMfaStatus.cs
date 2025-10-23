namespace CIPP.Api.Modules.Identity.Models;

public class UserMfaStatus {
    public bool Enabled { get; set; }
    public bool Enforced { get; set; }
    public List<string> Methods { get; set; } = new();
    public string DefaultMethod { get; set; } = string.Empty;
}