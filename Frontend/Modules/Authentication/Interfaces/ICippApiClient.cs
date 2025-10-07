using CIPP.Shared.DTOs;

namespace CIPP.Frontend.Modules.Authentication.Interfaces;

public interface ICippApiClient {
    Task<Response<T>> GetAsync<T>(string endpoint, string? apiVersion = null);
    Task<PagedResponse<T>> GetPagedAsync<T>(string endpoint, int pageNumber = 1, int pageSize = 50, bool noCache = false, string? apiVersion = null);
    Task<Response<T>> PostAsync<T>(string endpoint, object data, string? apiVersion = null);
    Task<Response<T>> PutAsync<T>(string endpoint, object data, string? apiVersion = null);
    Task<Response<bool>> DeleteAsync(string endpoint, string? apiVersion = null);
}
