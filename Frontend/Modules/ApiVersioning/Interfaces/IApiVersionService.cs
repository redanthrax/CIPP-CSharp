namespace CIPP.Frontend.Modules.ApiVersioning.Interfaces;

public interface IApiVersionService {
    string DefaultVersion { get; }
    string GetVersionedUrl(string endpoint, string? version = null);
    string GetNonVersionedUrl(string endpoint);
}
