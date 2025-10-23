using CIPP.Api.Data;
using CIPP.Api.Modules.Applications.Interfaces;
using CIPP.Api.Modules.Applications.Models;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Applications.Services;

public class PermissionSetService : IPermissionSetService {
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PermissionSetService> _logger;

    public PermissionSetService(ApplicationDbContext context, ILogger<PermissionSetService> logger) {
        _context = context;
        _logger = logger;
    }

    public async Task<PagedResponse<PermissionSetDto>> GetPermissionSetsAsync(PagingParameters? paging = null, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting permission sets");
        
        paging ??= new PagingParameters();
        var query = _context.GetEntitySet<AppPermissionSet>().AsQueryable();
        
        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(p => p.CreatedOn)
            .Skip(paging.Skip)
            .Take(paging.Take)
            .Select(p => MapToDto(p))
            .ToListAsync(cancellationToken);
        
        return new PagedResponse<PermissionSetDto> {
            Items = items,
            TotalCount = total,
            PageNumber = paging.PageNumber,
            PageSize = paging.PageSize
        };
    }

    public async Task<PermissionSetDto?> GetPermissionSetAsync(Guid id, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting permission set {PermissionSetId}", id);
        
        var permissionSet = await _context.GetEntitySet<AppPermissionSet>()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        
        return permissionSet != null ? MapToDto(permissionSet) : null;
    }

    public async Task<PermissionSetDto> CreatePermissionSetAsync(CreatePermissionSetDto createDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Creating permission set {TemplateName}", createDto.TemplateName);
        
        var permissionSet = new AppPermissionSet {
            Id = Guid.NewGuid(),
            TemplateName = createDto.TemplateName,
            Description = createDto.Description,
            PermissionsJson = createDto.PermissionsJson,
            CreatedOn = DateTime.UtcNow
        };
        
        _context.GetEntitySet<AppPermissionSet>().Add(permissionSet);
        await _context.SaveChangesAsync(cancellationToken);
        
        return MapToDto(permissionSet);
    }

    public async Task<PermissionSetDto> UpdatePermissionSetAsync(Guid id, UpdatePermissionSetDto updateDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating permission set {PermissionSetId}", id);
        
        var permissionSet = await _context.GetEntitySet<AppPermissionSet>()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        
        if (permissionSet == null) {
            throw new InvalidOperationException($"Permission set with ID {id} not found");
        }
        
        if (!string.IsNullOrEmpty(updateDto.TemplateName)) {
            permissionSet.TemplateName = updateDto.TemplateName;
        }
        
        if (updateDto.Description != null) {
            permissionSet.Description = updateDto.Description;
        }
        
        if (!string.IsNullOrEmpty(updateDto.PermissionsJson)) {
            permissionSet.PermissionsJson = updateDto.PermissionsJson;
        }
        
        permissionSet.UpdatedOn = DateTime.UtcNow;
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return MapToDto(permissionSet);
    }

    public async Task DeletePermissionSetAsync(Guid id, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Deleting permission set {PermissionSetId}", id);
        
        var permissionSet = await _context.GetEntitySet<AppPermissionSet>()
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        
        if (permissionSet == null) {
            throw new InvalidOperationException($"Permission set with ID {id} not found");
        }
        
        _context.GetEntitySet<AppPermissionSet>().Remove(permissionSet);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private static PermissionSetDto MapToDto(AppPermissionSet permissionSet) {
        return new PermissionSetDto {
            Id = permissionSet.Id,
            TemplateName = permissionSet.TemplateName,
            Description = permissionSet.Description,
            PermissionsJson = permissionSet.PermissionsJson,
            CreatedBy = permissionSet.CreatedBy,
            CreatedOn = permissionSet.CreatedOn,
            UpdatedBy = permissionSet.UpdatedBy,
            UpdatedOn = permissionSet.UpdatedOn
        };
    }
}
