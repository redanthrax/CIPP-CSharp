namespace CIPP.Shared.DTOs.SharePoint;

public class SharePointQuotaDto {
    public long StorageQuota { get; set; }
    public long StorageQuotaAllocated { get; set; }
    public double StorageQuotaGB { get; set; }
    public double StorageQuotaAllocatedGB { get; set; }
}
