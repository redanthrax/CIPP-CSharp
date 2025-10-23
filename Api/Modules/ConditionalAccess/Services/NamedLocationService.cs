using CIPP.Api.Modules.ConditionalAccess.Interfaces;
using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.ConditionalAccess;
using Microsoft.Graph.Beta.Models;

namespace CIPP.Api.Modules.ConditionalAccess.Services;

public class NamedLocationService : INamedLocationService {
    private readonly IMicrosoftGraphService _graphService;
    private readonly ILogger<NamedLocationService> _logger;

    public NamedLocationService(IMicrosoftGraphService graphService, ILogger<NamedLocationService> logger) {
        _graphService = graphService;
        _logger = logger;
    }

    public async Task<PagedResponse<NamedLocationDto>> GetNamedLocationsAsync(string tenantId, PagingParameters? paging = null, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting named locations for tenant {TenantId}", tenantId);
        
        paging ??= new PagingParameters();
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        var locations = await graphClient.Identity.ConditionalAccess.NamedLocations.GetAsync(cancellationToken: cancellationToken);
        
        if (locations?.Value == null) {
            return new PagedResponse<NamedLocationDto> {
                Items = new List<NamedLocationDto>(),
                TotalCount = 0,
                PageNumber = paging.PageNumber,
                PageSize = paging.PageSize
            };
        }

        var allLocations = locations.Value.Select(l => MapToLocationDto(l, tenantId)).ToList();
        var pagedLocations = allLocations.Skip(paging.Skip).Take(paging.Take).ToList();
        
        return new PagedResponse<NamedLocationDto> {
            Items = pagedLocations,
            TotalCount = allLocations.Count,
            PageNumber = paging.PageNumber,
            PageSize = paging.PageSize
        };
    }

    public async Task<NamedLocationDto?> GetNamedLocationAsync(string tenantId, string locationId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting named location {LocationId} for tenant {TenantId}", locationId, tenantId);
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        var location = await graphClient.Identity.ConditionalAccess.NamedLocations[locationId].GetAsync(cancellationToken: cancellationToken);
        
        return location != null ? MapToLocationDto(location, tenantId) : null;
    }

    public async Task<NamedLocationDto> CreateNamedLocationAsync(CreateNamedLocationDto createDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Creating named location {DisplayName} for tenant {TenantId}", createDto.DisplayName, createDto.TenantId);
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(createDto.TenantId);
        var location = MapToGraphLocation(createDto);
        
        var createdLocation = await graphClient.Identity.ConditionalAccess.NamedLocations.PostAsync(location, cancellationToken: cancellationToken);
        
        if (createdLocation == null) {
            throw new InvalidOperationException("Failed to create named location");
        }
        
        return MapToLocationDto(createdLocation, createDto.TenantId);
    }

    public async Task DeleteNamedLocationAsync(string tenantId, string locationId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Deleting named location {LocationId} for tenant {TenantId}", locationId, tenantId);
        
        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        await graphClient.Identity.ConditionalAccess.NamedLocations[locationId].DeleteAsync(cancellationToken: cancellationToken);
    }

    private static NamedLocationDto MapToLocationDto(NamedLocation location, string tenantId) {
        var dto = new NamedLocationDto {
            Id = location.Id ?? string.Empty,
            DisplayName = location.DisplayName ?? string.Empty,
            CreatedDateTime = location.CreatedDateTime?.DateTime,
            ModifiedDateTime = location.ModifiedDateTime?.DateTime,
            TenantId = tenantId,
            OdataType = location.OdataType ?? string.Empty
        };

        if (location is IpNamedLocation ipLocation) {
            dto.IsTrusted = ipLocation.IsTrusted ?? false;
            dto.IpRanges = ipLocation.IpRanges?
                .Select(r => r is IPv4CidrRange ipv4 ? ipv4.CidrAddress : 
                            r is IPv6CidrRange ipv6 ? ipv6.CidrAddress : string.Empty)
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => s!)
                .ToList();
        }

        if (location is CountryNamedLocation countryLocation) {
            dto.CountriesAndRegions = countryLocation.CountriesAndRegions?.ToList();
            dto.IncludeUnknownCountriesAndRegions = countryLocation.IncludeUnknownCountriesAndRegions;
        }

        return dto;
    }

    private static NamedLocation MapToGraphLocation(CreateNamedLocationDto dto) {
        if (dto.LocationType.Equals("ipRange", StringComparison.OrdinalIgnoreCase)) {
            return new IpNamedLocation {
                DisplayName = dto.DisplayName,
                IsTrusted = dto.IsTrusted,
                IpRanges = dto.IpRanges?.Select(ip => {
                    IpRange range = new IPv4CidrRange {
                        CidrAddress = ip
                    };
                    return range;
                }).ToList()
            };
        }

        if (dto.LocationType.Equals("country", StringComparison.OrdinalIgnoreCase)) {
            return new CountryNamedLocation {
                DisplayName = dto.DisplayName,
                CountriesAndRegions = dto.CountriesAndRegions,
                IncludeUnknownCountriesAndRegions = dto.IncludeUnknownCountriesAndRegions
            };
        }

        throw new ArgumentException($"Invalid location type: {dto.LocationType}");
    }
}
