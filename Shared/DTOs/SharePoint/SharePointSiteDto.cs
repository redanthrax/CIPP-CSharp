namespace CIPP.Shared.DTOs.SharePoint;

public class SharePointSiteDto {
    public string SiteId { get; set; } = string.Empty;
    public string WebId { get; set; } = string.Empty;
    public DateTime? CreatedDateTime { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string WebUrl { get; set; } = string.Empty;
    public string? OwnerDisplayName { get; set; }
    public string? OwnerPrincipalName { get; set; }
    public DateTime? LastActivityDate { get; set; }
    public int FileCount { get; set; }
    public double StorageUsedInGigabytes { get; set; }
    public double StorageAllocatedInGigabytes { get; set; }
    public long StorageUsedInBytes { get; set; }
    public long StorageAllocatedInBytes { get; set; }
    public string? RootWebTemplate { get; set; }
    public DateTime? ReportRefreshDate { get; set; }
    public string? AutoMapUrl { get; set; }
    public bool IsPersonalSite { get; set; }
}
