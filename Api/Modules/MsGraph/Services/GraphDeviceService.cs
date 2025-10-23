using Microsoft.Graph.Beta;
using Microsoft.Graph.Beta.Models;
using CIPP.Shared.DTOs;

namespace CIPP.Api.Modules.MsGraph.Services;

public class GraphDeviceService {
    private readonly MicrosoftGraphService _graphService;
    private readonly CachedGraphRequestHandler _cacheHandler;
    private readonly GraphExceptionHandler _exceptionHandler;

    public GraphDeviceService(MicrosoftGraphService graphService, CachedGraphRequestHandler cacheHandler, GraphExceptionHandler exceptionHandler) {
        _graphService = graphService;
        _cacheHandler = cacheHandler;
        _exceptionHandler = exceptionHandler;
    }

    public async Task<DeviceCollectionResponse?> ListDevicesAsync(string? tenantId = null, string? filter = null, string[]? select = null, PagingParameters? paging = null) {
        return await _exceptionHandler.HandleAsync(async () => {
            var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
            paging ??= new PagingParameters();
            
            return await _cacheHandler.ExecuteAsync(
                tenantId ?? "default",
                () => graphClient.Devices.GetAsync(requestConfiguration => {
                    if (!string.IsNullOrEmpty(filter))
                        requestConfiguration.QueryParameters.Filter = filter;
                    if (select != null && select.Length > 0)
                        requestConfiguration.QueryParameters.Select = select;
                    requestConfiguration.QueryParameters.Top = paging.PageSize;
                }),
                "devices",
                "GET",
                new { filter, select = select != null ? string.Join(",", select) : null, pageNumber = paging.PageNumber, pageSize = paging.PageSize, skipToken = paging.SkipToken }
            );
        }, tenantId, "listing devices");
    }

    public async Task<Device?> GetDeviceAsync(string deviceId, string? tenantId = null) {
        return await _exceptionHandler.HandleAsync(async () => {
            var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
            return await _cacheHandler.ExecuteAsync(
                tenantId ?? "default",
                () => graphClient.Devices[deviceId].GetAsync(),
                $"devices/{deviceId}",
                "GET"
            );
        }, tenantId, $"getting device {deviceId}");
    }

    public async Task<Device?> UpdateDeviceAsync(string deviceId, Device device, string? tenantId = null) {
        return await _exceptionHandler.HandleAsync(async () => {
            var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
            return await _cacheHandler.ExecuteAsync(
                tenantId ?? "default",
                () => graphClient.Devices[deviceId].PatchAsync(device),
                $"devices/{deviceId}",
                "PATCH"
            );
        }, tenantId, $"updating device {deviceId}");
    }

    public async Task DeleteDeviceAsync(string deviceId, string? tenantId = null) {
        await _exceptionHandler.HandleAsync(async () => {
            var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
            await _cacheHandler.ExecuteAsync(
                tenantId ?? "default",
                () => graphClient.Devices[deviceId].DeleteAsync(),
                $"devices/{deviceId}",
                "DELETE"
            );
            return Task.CompletedTask;
        }, tenantId, $"deleting device {deviceId}");
    }
}
