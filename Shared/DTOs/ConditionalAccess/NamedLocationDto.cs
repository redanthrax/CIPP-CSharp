namespace CIPP.Shared.DTOs.ConditionalAccess;

public class NamedLocationDto {
    public string Id { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public DateTime? CreatedDateTime { get; set; }
    public DateTime? ModifiedDateTime { get; set; }
    public bool IsTrusted { get; set; }
    public string TenantId { get; set; } = string.Empty;
    public string OdataType { get; set; } = string.Empty;
    
    public List<string>? IpRanges { get; set; }
    public List<string>? CountriesAndRegions { get; set; }
    public bool? IncludeUnknownCountriesAndRegions { get; set; }
}
