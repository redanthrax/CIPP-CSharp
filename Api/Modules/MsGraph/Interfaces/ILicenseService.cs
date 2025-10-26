namespace CIPP.Api.Modules.MsGraph.Interfaces;

public interface ILicenseService {
    Task AssignLicensesAsync(Guid tenantId, string userId, List<string> licenseSkuIds, CancellationToken cancellationToken = default);
    Task RemoveLicensesAsync(Guid tenantId, string userId, List<string> licenseSkuIds, CancellationToken cancellationToken = default);
    Task ReplaceLicensesAsync(Guid tenantId, string userId, List<string> addLicenseSkuIds, List<string> removeLicenseSkuIds, CancellationToken cancellationToken = default);
    Task<List<string>> GetUserLicensesAsync(Guid tenantId, string userId, CancellationToken cancellationToken = default);
}