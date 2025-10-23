namespace CIPP.Api.Modules.MsGraph.Interfaces;

public interface ILicenseService {
    Task AssignLicensesAsync(string tenantId, string userId, List<string> licenseSkuIds, CancellationToken cancellationToken = default);
    Task RemoveLicensesAsync(string tenantId, string userId, List<string> licenseSkuIds, CancellationToken cancellationToken = default);
    Task ReplaceLicensesAsync(string tenantId, string userId, List<string> addLicenseSkuIds, List<string> removeLicenseSkuIds, CancellationToken cancellationToken = default);
    Task<List<string>> GetUserLicensesAsync(string tenantId, string userId, CancellationToken cancellationToken = default);
}