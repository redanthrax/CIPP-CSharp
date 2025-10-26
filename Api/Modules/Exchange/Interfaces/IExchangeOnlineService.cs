namespace CIPP.Api.Modules.Exchange.Interfaces;

public interface IExchangeOnlineService {
    Task<T?> ExecuteCmdletAsync<T>(Guid tenantId, string cmdlet, Dictionary<string, object>? parameters = null, CancellationToken cancellationToken = default) where T : class;
    Task<List<T>> ExecuteCmdletListAsync<T>(Guid tenantId, string cmdlet, Dictionary<string, object>? parameters = null, CancellationToken cancellationToken = default) where T : class;
    Task ExecuteCmdletNoResultAsync(Guid tenantId, string cmdlet, Dictionary<string, object>? parameters = null, CancellationToken cancellationToken = default);
}
