using CIPP.Api.Data;
using CIPP.Api.Modules.Applications.Interfaces;
using CIPP.Api.Modules.Applications.Models;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Applications;
using Microsoft.EntityFrameworkCore;

namespace CIPP.Api.Modules.Applications.Services;

public class AppTemplateService : IAppTemplateService {
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AppTemplateService> _logger;

    public AppTemplateService(ApplicationDbContext context, ILogger<AppTemplateService> logger) {
        _context = context;
        _logger = logger;
    }

    public async Task<PagedResponse<AppTemplateDto>> GetTemplatesAsync(PagingParameters? paging = null, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting app templates");
        
        paging ??= new PagingParameters();
        var query = _context.GetEntitySet<AppTemplate>().AsQueryable();
        
        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(t => t.CreatedOn)
            .Skip(paging.Skip)
            .Take(paging.Take)
            .Select(t => MapToDto(t))
            .ToListAsync(cancellationToken);
        
        return new PagedResponse<AppTemplateDto> {
            Items = items,
            TotalCount = total,
            PageNumber = paging.PageNumber,
            PageSize = paging.PageSize
        };
    }

    public async Task<AppTemplateDto?> GetTemplateAsync(Guid id, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting app template {TemplateId}", id);
        
        var template = await _context.GetEntitySet<AppTemplate>()
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        
        return template != null ? MapToDto(template) : null;
    }

    public async Task<AppTemplateDto> CreateTemplateAsync(CreateAppTemplateDto createDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Creating app template {TemplateName}", createDto.TemplateName);
        
        var template = new AppTemplate {
            Id = Guid.NewGuid(),
            TemplateName = createDto.TemplateName,
            Description = createDto.Description,
            TemplateType = createDto.TemplateType,
            TemplateJson = createDto.TemplateJson,
            CreatedOn = DateTime.UtcNow
        };
        
        _context.GetEntitySet<AppTemplate>().Add(template);
        await _context.SaveChangesAsync(cancellationToken);
        
        return MapToDto(template);
    }

    public async Task<AppTemplateDto> UpdateTemplateAsync(Guid id, UpdateAppTemplateDto updateDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating app template {TemplateId}", id);
        
        var template = await _context.GetEntitySet<AppTemplate>()
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        
        if (template == null) {
            throw new InvalidOperationException($"Template with ID {id} not found");
        }
        
        if (!string.IsNullOrEmpty(updateDto.TemplateName)) {
            template.TemplateName = updateDto.TemplateName;
        }
        
        if (updateDto.Description != null) {
            template.Description = updateDto.Description;
        }
        
        if (!string.IsNullOrEmpty(updateDto.TemplateType)) {
            template.TemplateType = updateDto.TemplateType;
        }
        
        if (!string.IsNullOrEmpty(updateDto.TemplateJson)) {
            template.TemplateJson = updateDto.TemplateJson;
        }
        
        template.UpdatedOn = DateTime.UtcNow;
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return MapToDto(template);
    }

    public async Task DeleteTemplateAsync(Guid id, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Deleting app template {TemplateId}", id);
        
        var template = await _context.GetEntitySet<AppTemplate>()
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        
        if (template == null) {
            throw new InvalidOperationException($"Template with ID {id} not found");
        }
        
        _context.GetEntitySet<AppTemplate>().Remove(template);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private static AppTemplateDto MapToDto(AppTemplate template) {
        return new AppTemplateDto {
            Id = template.Id,
            TemplateName = template.TemplateName,
            Description = template.Description,
            TemplateType = template.TemplateType,
            TemplateJson = template.TemplateJson,
            CreatedBy = template.CreatedBy,
            CreatedOn = template.CreatedOn,
            UpdatedBy = template.UpdatedBy,
            UpdatedOn = template.UpdatedOn
        };
    }
}
