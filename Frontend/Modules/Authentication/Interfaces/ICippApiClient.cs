using CIPP.Shared.DTOs;

namespace CIPP.Frontend.Modules.Authentication.Interfaces;

public interface ICippApiClient {
    Task<Response<T>> GetAsync<T>(string endpoint);
    Task<PagedResponse<T>> GetPagedAsync<T>(string endpoint, int pageNumber = 1, int pageSize = 50, bool noCache = false);
    Task<Response<T>> PostAsync<T>(string endpoint, object data);
    Task<Response<T>> PutAsync<T>(string endpoint, object data);
    Task<Response<bool>> DeleteAsync(string endpoint);
}