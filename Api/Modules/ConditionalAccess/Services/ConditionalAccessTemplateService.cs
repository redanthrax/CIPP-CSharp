using CIPP.Api.Data;
using CIPP.Api.Modules.ConditionalAccess.Interfaces;
using CIPP.Api.Modules.ConditionalAccess.Models;
using CIPP.Shared.DTOs.ConditionalAccess;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CIPP.Api.Modules.ConditionalAccess.Services;

public class ConditionalAccessTemplateService : IConditionalAccessTemplateService {
    private readonly ApplicationDbContext _context;
    private readonly IConditionalAccessPolicyService _policyService;
    private readonly ILogger<ConditionalAccessTemplateService> _logger;

    public ConditionalAccessTemplateService(
        ApplicationDbContext context,
        IConditionalAccessPolicyService policyService,
        ILogger<ConditionalAccessTemplateService> logger) {
        _context = context;
        _policyService = policyService;
        _logger = logger;
    }

    public async Task<List<ConditionalAccessTemplateDto>> GetTemplatesAsync(CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting all conditional access templates");

        var templates = await _context.GetEntitySet<ConditionalAccessTemplate>()
            .OrderByDescending(t => t.CreatedOn)
            .ToListAsync(cancellationToken);

        return templates.Select(MapToDto).ToList();
    }

    public async Task<ConditionalAccessTemplateDto?> GetTemplateAsync(Guid id, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting conditional access template {TemplateId}", id);

        var template = await _context.GetEntitySet<ConditionalAccessTemplate>()
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        return template != null ? MapToDto(template) : null;
    }

    public async Task<ConditionalAccessTemplateDto> CreateTemplateAsync(CreateConditionalAccessTemplateDto createDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Creating conditional access template {TemplateName}", createDto.TemplateName);

        var template = new ConditionalAccessTemplate {
            Id = Guid.NewGuid(),
            TemplateName = createDto.TemplateName,
            Description = createDto.Description,
            TemplateJson = JsonSerializer.Serialize(createDto.PolicyData),
            CreatedBy = createDto.CreatedBy,
            CreatedOn = DateTime.UtcNow
        };

        _context.GetEntitySet<ConditionalAccessTemplate>().Add(template);
        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(template);
    }

    public async Task<ConditionalAccessTemplateDto> UpdateTemplateAsync(Guid id, UpdateConditionalAccessTemplateDto updateDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating conditional access template {TemplateId}", id);

        var template = await _context.GetEntitySet<ConditionalAccessTemplate>()
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

        if (updateDto.PolicyData != null) {
            template.TemplateJson = JsonSerializer.Serialize(updateDto.PolicyData);
        }

        template.UpdatedBy = updateDto.UpdatedBy;
        template.UpdatedOn = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(template);
    }

    public async Task DeleteTemplateAsync(Guid id, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Deleting conditional access template {TemplateId}", id);

        var template = await _context.GetEntitySet<ConditionalAccessTemplate>()
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (template == null) {
            throw new InvalidOperationException($"Template with ID {id} not found");
        }

        _context.GetEntitySet<ConditionalAccessTemplate>().Remove(template);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<ConditionalAccessPolicyDto> DeployTemplateAsync(DeployConditionalAccessTemplateDto deployDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Deploying conditional access template {TemplateId} to tenant {TenantId}", deployDto.TemplateId, deployDto.TenantId);

        var template = await _context.GetEntitySet<ConditionalAccessTemplate>()
            .FirstOrDefaultAsync(t => t.Id == deployDto.TemplateId, cancellationToken);

        if (template == null) {
            throw new InvalidOperationException($"Template with ID {deployDto.TemplateId} not found");
        }

        var policyData = JsonSerializer.Deserialize<ConditionalAccessPolicyDto>(template.TemplateJson);
        if (policyData == null) {
            throw new InvalidOperationException($"Failed to deserialize template data for template {deployDto.TemplateId}");
        }

        var createDto = new CreateConditionalAccessPolicyDto {
            TenantId = deployDto.TenantId,
            DisplayName = policyData.DisplayName,
            Description = policyData.Description ?? $"Deployed from template: {template.TemplateName}",
            State = deployDto.State ?? policyData.State,
            Conditions = policyData.Conditions,
            GrantControls = policyData.GrantControls,
            SessionControls = policyData.SessionControls
        };

        if (deployDto.Overwrite) {
            var existingPolicies = await _policyService.GetPoliciesAsync(deployDto.TenantId, cancellationToken: cancellationToken);
            var existingPolicy = existingPolicies.Items.FirstOrDefault(p => p.DisplayName == policyData.DisplayName);

            if (existingPolicy != null) {
                _logger.LogInformation("Overwriting existing policy {PolicyId} with template {TemplateId}", existingPolicy.Id, deployDto.TemplateId);
                var updateDto = new UpdateConditionalAccessPolicyDto {
                    DisplayName = createDto.DisplayName,
                    Description = createDto.Description,
                    State = createDto.State,
                    Conditions = createDto.Conditions,
                    GrantControls = createDto.GrantControls,
                    SessionControls = createDto.SessionControls
                };
                return await _policyService.UpdatePolicyAsync(deployDto.TenantId, existingPolicy.Id, updateDto, cancellationToken);
            }
        }

        return await _policyService.CreatePolicyAsync(createDto, cancellationToken);
    }

    private static ConditionalAccessTemplateDto MapToDto(ConditionalAccessTemplate template) {
        ConditionalAccessPolicyDto? policyData = null;
        try {
            policyData = JsonSerializer.Deserialize<ConditionalAccessPolicyDto>(template.TemplateJson);
        } catch {
        }

        return new ConditionalAccessTemplateDto {
            Id = template.Id,
            TemplateName = template.TemplateName,
            Description = template.Description,
            PolicyData = policyData,
            CreatedBy = template.CreatedBy,
            CreatedOn = template.CreatedOn,
            UpdatedBy = template.UpdatedBy,
            UpdatedOn = template.UpdatedOn
        };
    }
}
