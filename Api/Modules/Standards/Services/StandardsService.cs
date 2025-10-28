using CIPP.Api.Data;
using CIPP.Api.Extensions;
using CIPP.Api.Modules.Standards.Interfaces;
using CIPP.Api.Modules.Standards.Models;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Standards;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CIPP.Api.Modules.Standards.Services;

public class StandardsService : IStandardsService {
    private readonly ApplicationDbContext _context;
    private readonly ILogger<StandardsService> _logger;
    private readonly StandardExecutorFactory _executorFactory;

    public StandardsService(ApplicationDbContext context, ILogger<StandardsService> logger, StandardExecutorFactory executorFactory) {
        _context = context;
        _logger = logger;
        _executorFactory = executorFactory;
    }

    public async Task<PagedResponse<StandardTemplateDto>> GetTemplatesAsync(string? type, string? category, PagingParameters pagingParams, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting standard templates: type={Type}, category={Category}", type, category);

        var query = _context.Set<StandardTemplate>().AsQueryable();

        if (!string.IsNullOrEmpty(type)) {
            query = query.Where(t => t.Type == type);
        }

        if (!string.IsNullOrEmpty(category)) {
            query = query.Where(t => t.Category == category);
        }

        var templates = await query
            .OrderByDescending(t => t.CreatedDate)
            .ToListAsync(cancellationToken);

        var templateDtos = templates.Select(MapToDto).ToList();

        return templateDtos.ToPagedResponse(pagingParams);
    }

    public async Task<StandardTemplateDto?> GetTemplateAsync(Guid templateId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting standard template {TemplateId}", templateId);

        var template = await _context.Set<StandardTemplate>()
            .FirstOrDefaultAsync(t => t.Id == templateId, cancellationToken);

        return template == null ? null : MapToDto(template);
    }

    public async Task<StandardTemplateDto> CreateTemplateAsync(CreateStandardTemplateDto createDto, string? createdBy, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Creating standard template {Name}", createDto.Name);

        var template = new StandardTemplate {
            Id = Guid.NewGuid(),
            Name = createDto.Name,
            Description = createDto.Description,
            Type = createDto.Type,
            Category = createDto.Category,
            Configuration = createDto.Configuration,
            IsEnabled = createDto.IsEnabled,
            IsGlobal = createDto.IsGlobal,
            CreatedBy = createdBy,
            CreatedDate = DateTime.UtcNow
        };

        _context.Set<StandardTemplate>().Add(template);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created standard template {TemplateId}", template.Id);

        return MapToDto(template);
    }

    public async Task<StandardTemplateDto?> UpdateTemplateAsync(Guid templateId, UpdateStandardTemplateDto updateDto, string? modifiedBy, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating standard template {TemplateId}", templateId);

        var template = await _context.Set<StandardTemplate>()
            .FirstOrDefaultAsync(t => t.Id == templateId, cancellationToken);

        if (template == null) {
            return null;
        }

        if (!string.IsNullOrEmpty(updateDto.Name)) {
            template.Name = updateDto.Name;
        }

        if (updateDto.Description != null) {
            template.Description = updateDto.Description;
        }

        if (updateDto.Configuration != null) {
            template.Configuration = updateDto.Configuration;
        }

        if (updateDto.IsEnabled.HasValue) {
            template.IsEnabled = updateDto.IsEnabled.Value;
        }

        template.ModifiedBy = modifiedBy;
        template.ModifiedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Updated standard template {TemplateId}", templateId);

        return MapToDto(template);
    }

    public async Task<bool> DeleteTemplateAsync(Guid templateId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Deleting standard template {TemplateId}", templateId);

        var template = await _context.Set<StandardTemplate>()
            .FirstOrDefaultAsync(t => t.Id == templateId, cancellationToken);

        if (template == null) {
            return false;
        }

        _context.Set<StandardTemplate>().Remove(template);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Deleted standard template {TemplateId}", templateId);

        return true;
    }

    public async Task<PagedResponse<StandardExecutionDto>> GetExecutionHistoryAsync(Guid? templateId, Guid? tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting execution history: templateId={TemplateId}, tenantId={TenantId}", templateId, tenantId);

        var query = _context.Set<StandardExecution>()
            .Include(e => e.Template)
            .AsQueryable();

        if (templateId.HasValue) {
            query = query.Where(e => e.TemplateId == templateId.Value);
        }

        if (tenantId.HasValue) {
            query = query.Where(e => e.TenantId == tenantId.Value);
        }

        var executions = await query
            .OrderByDescending(e => e.ExecutedDate)
            .ToListAsync(cancellationToken);

        var executionDtos = executions.Select(e => new StandardExecutionDto {
            Id = e.Id,
            TemplateId = e.TemplateId,
            TemplateName = e.Template.Name,
            TenantId = e.TenantId,
            Status = e.Status,
            Result = e.Result,
            ErrorMessage = e.ErrorMessage,
            ExecutedDate = e.ExecutedDate,
            ExecutedBy = e.ExecutedBy,
            DurationMs = e.DurationMs
        }).ToList();

        return executionDtos.ToPagedResponse(pagingParams);
    }

    public async Task<List<StandardResultDto>> DeployStandardAsync(DeployStandardDto deployDto, string? executedBy, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Deploying standard {TemplateId} to {TenantCount} tenants", deployDto.TemplateId, deployDto.TenantIds.Length);

        var template = await _context.Set<StandardTemplate>()
            .FirstOrDefaultAsync(t => t.Id == deployDto.TemplateId, cancellationToken);

        if (template == null) {
            throw new InvalidOperationException($"Template {deployDto.TemplateId} not found");
        }

        var results = new List<StandardResultDto>();

        foreach (var tenantId in deployDto.TenantIds) {
            var stopwatch = Stopwatch.StartNew();
            var execution = new StandardExecution {
                Id = Guid.NewGuid(),
                TemplateId = template.Id,
                TenantId = tenantId,
                ExecutedBy = executedBy,
                ExecutedDate = DateTime.UtcNow
            };

            try {
                var config = deployDto.OverrideConfiguration && !string.IsNullOrEmpty(deployDto.ConfigurationOverride)
                    ? deployDto.ConfigurationOverride
                    : template.Configuration;

                var executor = _executorFactory.GetExecutor(template.Type);
                if (executor == null) {
                    throw new InvalidOperationException($"No executor found for standard type: {template.Type}");
                }

                _logger.LogInformation("Executing standard {TemplateId} on tenant {TenantId} using {ExecutorType}", template.Id, tenantId, template.Type);
                var result = await executor.ExecuteAsync(tenantId, config, executedBy, cancellationToken);

                execution.Status = result.Success ? "Success" : "Failed";
                execution.Result = result.Message;
                execution.ErrorMessage = result.ErrorDetails;
                stopwatch.Stop();
                execution.DurationMs = result.DurationMs.HasValue ? result.DurationMs.Value : (int)stopwatch.ElapsedMilliseconds;

                results.Add(result);
            } catch (Exception ex) {
                _logger.LogError(ex, "Error deploying standard {TemplateId} to tenant {TenantId}", template.Id, tenantId);

                execution.Status = "Failed";
                execution.ErrorMessage = ex.Message;
                stopwatch.Stop();
                execution.DurationMs = (int)stopwatch.ElapsedMilliseconds;

                results.Add(new StandardResultDto {
                    ExecutionId = execution.Id,
                    TenantId = tenantId,
                    Success = false,
                    Message = ex.Message,
                    DurationMs = execution.DurationMs.Value
                });
            }

            _context.Set<StandardExecution>().Add(execution);
        }

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Deployed standard {TemplateId} to {TenantCount} tenants: {SuccessCount} succeeded, {FailCount} failed",
            deployDto.TemplateId, deployDto.TenantIds.Length, results.Count(r => r.Success), results.Count(r => !r.Success));

        return results;
    }

    private static StandardTemplateDto MapToDto(StandardTemplate template) {
        return new StandardTemplateDto {
            Id = template.Id,
            Name = template.Name,
            Description = template.Description,
            Type = template.Type,
            Category = template.Category,
            Configuration = template.Configuration,
            IsEnabled = template.IsEnabled,
            IsGlobal = template.IsGlobal,
            CreatedBy = template.CreatedBy,
            CreatedDate = template.CreatedDate,
            ModifiedBy = template.ModifiedBy,
            ModifiedDate = template.ModifiedDate
        };
    }
}
