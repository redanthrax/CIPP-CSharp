namespace CIPP.Shared.DTOs.ConditionalAccess;

public class CreateNamedLocationDto {
    public Guid TenantId { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public bool IsTrusted { get; set; }
    public string LocationType { get; set; } = "ipRange";
    
    public List<string>? IpRanges { get; set; }
    public List<string>? CountriesAndRegions { get; set; }
    public bool? IncludeUnknownCountriesAndRegions { get; set; }
}
