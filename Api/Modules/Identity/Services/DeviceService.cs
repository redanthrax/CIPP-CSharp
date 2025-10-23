using CIPP.Api.Modules.Identity.Interfaces;
using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Api.Modules.MsGraph.Services;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Identity;
using Microsoft.Graph.Beta.Models;
using Microsoft.AspNetCore.WebUtilities;

namespace CIPP.Api.Modules.Identity.Services;

public class DeviceService : IDeviceService {
    private readonly GraphDeviceService _graphDeviceService;
    private readonly GraphUserService _graphUserService;
    private readonly IMicrosoftGraphService _graphService;
    private readonly ILogger<DeviceService> _logger;

    public DeviceService(
        GraphDeviceService graphDeviceService,
        GraphUserService graphUserService,
        IMicrosoftGraphService graphService, 
        ILogger<DeviceService> logger) {
        _graphDeviceService = graphDeviceService;
        _graphUserService = graphUserService;
        _graphService = graphService;
        _logger = logger;
    }

    public async Task<PagedResponse<DeviceDto>> GetDevicesAsync(string tenantId, PagingParameters? paging = null, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting devices for tenant {TenantId}", tenantId);
        
        paging ??= new PagingParameters();
        var response = await _graphDeviceService.ListDevicesAsync(tenantId, paging: paging);
        
        if (response?.Value == null) {
            return new PagedResponse<DeviceDto> {
                Items = new List<DeviceDto>(),
                TotalCount = 0,
                PageNumber = paging.PageNumber,
                PageSize = paging.PageSize
            };
        }

        var devices = response.Value.Select(device => MapToDeviceDto(device, tenantId)).ToList();
        
        return new PagedResponse<DeviceDto> {
            Items = devices,
            TotalCount = (int)(response.OdataCount ?? devices.Count),
            PageNumber = paging.PageNumber,
            PageSize = paging.PageSize,
            SkipToken = response.OdataNextLink != null ? ExtractSkipToken(response.OdataNextLink) : null
        };
    }
    
    private static string? ExtractSkipToken(string? nextLink) {
        if (string.IsNullOrEmpty(nextLink)) return null;
        var uri = new Uri(nextLink);
        var query = QueryHelpers.ParseQuery(uri.Query);
        return query.TryGetValue("$skiptoken", out var token) ? token.ToString() : null;
    }

    public async Task<DeviceDto?> GetDeviceAsync(string tenantId, string deviceId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting device {DeviceId} for tenant {TenantId}", deviceId, tenantId);
        
        var device = await _graphDeviceService.GetDeviceAsync(deviceId, tenantId);
        
        return device != null ? MapToDeviceDto(device, tenantId) : null;
    }

    public async Task DeleteDeviceAsync(string tenantId, string deviceId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Deleting device {DeviceId} for tenant {TenantId}", deviceId, tenantId);

        await _graphDeviceService.DeleteDeviceAsync(deviceId, tenantId);
    }

    public async Task DisableDeviceAsync(string tenantId, string deviceId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Disabling device {DeviceId} for tenant {TenantId}", deviceId, tenantId);

        var device = new Device {
            AccountEnabled = false
        };

        await _graphDeviceService.UpdateDeviceAsync(deviceId, device, tenantId);
    }

    public async Task EnableDeviceAsync(string tenantId, string deviceId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Enabling device {DeviceId} for tenant {TenantId}", deviceId, tenantId);

        var device = new Device {
            AccountEnabled = true
        };

        await _graphDeviceService.UpdateDeviceAsync(deviceId, device, tenantId);
    }

    public async Task<IEnumerable<DeviceDto>> GetUserDevicesAsync(string tenantId, string userId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting devices for user {UserId} in tenant {TenantId}", userId, tenantId);

        try {
            var registeredDevices = await _graphUserService.GetUserRegisteredDevicesAsync(userId, tenantId);
            var devices = new List<DeviceDto>();
            
            if (registeredDevices?.Value != null) {
                foreach (var registeredDevice in registeredDevices.Value) {
                    if (registeredDevice is Device device) {
                        devices.Add(MapToDeviceDto(device, tenantId));
                    }
                }
            }

            var ownedDevices = await _graphUserService.GetUserOwnedDevicesAsync(userId, tenantId);
            if (ownedDevices?.Value != null) {
                foreach (var ownedDevice in ownedDevices.Value) {
                    if (ownedDevice is Device device && !devices.Any(d => d.Id == device.Id)) {
                        devices.Add(MapToDeviceDto(device, tenantId));
                    }
                }
            }

            return devices;
        }
        catch (Exception ex) {
            _logger.LogWarning(ex, "Failed to get devices for user {UserId}", userId);
            return Enumerable.Empty<DeviceDto>();
        }
    }

    private static DeviceDto MapToDeviceDto(Device device, string tenantId) {
        return new DeviceDto {
            Id = device.Id ?? "",
            DisplayName = device.DisplayName ?? "",
            DeviceId = device.DeviceId ?? "",
            OperatingSystem = device.OperatingSystem ?? "",
            OperatingSystemVersion = device.OperatingSystemVersion ?? "",
            IsCompliant = device.IsCompliant ?? false,
            IsManaged = device.IsManaged ?? false,
            TrustType = device.TrustType ?? "",
            ApproximateLastSignInDateTime = device.ApproximateLastSignInDateTime?.DateTime,
            RegistrationDateTime = device.RegistrationDateTime?.DateTime,
            AccountEnabled = device.AccountEnabled ?? false,
            Manufacturer = device.Manufacturer,
            Model = device.Model,
            PhysicalIds = device.PhysicalIds?.ToList() ?? new List<string>(),
            TenantId = tenantId
        };
    }
}