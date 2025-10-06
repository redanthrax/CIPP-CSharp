namespace CIPP.Api.Modules.Tenants.Models;

public class TenantCapabilities
{
    public bool HasExchange { get; set; }
    public bool HasSharePoint { get; set; }
    public bool HasTeams { get; set; }
    public bool HasIntune { get; set; }
    public bool HasDefender { get; set; }
    public List<string> Licenses { get; set; } = new();
}