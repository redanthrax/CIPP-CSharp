namespace CIPP.Api.Modules.Exchange.Interfaces;

public interface IExchangeOnlineService {
    Task<T?> ExecuteCmdletAsync<T>(string tenantId, string cmdlet, Dictionary<string, object>? parameters = null, CancellationToken cancellationToken = default) where T : class;
    Task<List<T>> ExecuteCmdletListAsync<T>(string tenantId, string cmdlet, Dictionary<string, object>? parameters = null, CancellationToken cancellationToken = default) where T : class;
    Task ExecuteCmdletNoResultAsync(string tenantId, string cmdlet, Dictionary<string, object>? parameters = null, CancellationToken cancellationToken = default);
}
