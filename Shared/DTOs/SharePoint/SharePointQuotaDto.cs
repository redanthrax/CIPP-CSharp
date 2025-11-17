namespace CIPP.Shared.DTOs.SharePoint;

public class SharePointQuotaDto {
    public long GeoUsedStorageMB { get; set; }
    public long TenantStorageMB { get; set; }
    public int Percentage { get; set; }
    public string Dashboard { get; set; } = string.Empty;
}
