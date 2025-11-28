using CIPP.Api.Data;
using CIPP.Api.Modules.Alerts.Interfaces;
using CIPP.Api.Modules.Alerts.Models;
using Fluid;
using Fluid.Values;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CIPP.Api.Modules.Alerts.Services;

public class AlertTemplateService : IAlertTemplateService {
    private readonly ApplicationDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AlertTemplateService> _logger;
    private readonly FluidParser _parser;
    private static readonly TemplateOptions _templateOptions;

    static AlertTemplateService() {
        _templateOptions = new TemplateOptions();
        _templateOptions.MemberAccessStrategy.Register<Dictionary<string, object>>();
    }

    public AlertTemplateService(
        ApplicationDbContext dbContext,
        IConfiguration configuration,
        ILogger<AlertTemplateService> logger) {
        _dbContext = dbContext;
        _configuration = configuration;
        _logger = logger;
        _parser = new FluidParser();
    }

    public async Task<AlertTemplate> GenerateAuditLogTemplateAsync(
        Dictionary<string, object> auditData, 
        string tenantId, 
        string? alertComment = null) {
        var operation = auditData.TryGetValue("Operation", out var op) ? op.ToString() : "Unknown";
        
        var templateType = operation switch {
            "New-InboxRule" => "NewInboxRule",
            "Set-InboxRule" => "SetInboxRule",
            "Add member to role." => "RoleChange",
            "UserLoggedIn" => "UserLoggedIn",
            _ => "Generic"
        };

        var template = await GetOrCreateTemplateAsync(templateType);
        return await RenderTemplateAsync(template, auditData, tenantId, alertComment);
    }

    public string GenerateHtmlEmail(AlertTemplate template, string? alertComment = null) {
        try {
            var baseTemplate = GetBaseTemplate();
            
            if (!_parser.TryParse(baseTemplate, out var fluidTemplate, out var error)) {
                _logger.LogError("Failed to parse base template: {Error}", error);
                return GenerateFallbackHtml(template, alertComment);
            }

            var context = new TemplateContext(new {
                title = template.Title,
                content = template.HtmlContent,
                alert_comment = alertComment,
                button_url = template.ButtonUrl,
                button_text = template.ButtonText
            }, _templateOptions);

            return fluidTemplate.Render(context);
        } catch (Exception ex) {
            _logger.LogError(ex, "Error generating HTML email");
            return GenerateFallbackHtml(template, alertComment);
        }
    }

    public string GenerateJsonPayload(AlertTemplate template, string? alertComment = null) {
        var payload = new {
            title = template.Title,
            buttonUrl = template.ButtonUrl,
            buttonText = template.ButtonText,
            data = template.Data,
            alertComment = alertComment
        };
        
        return JsonSerializer.Serialize(payload, new JsonSerializerOptions {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }

    private async Task<EmailTemplate> GetOrCreateTemplateAsync(string templateType) {
        var template = await _dbContext.GetEntitySet<EmailTemplate>()
            .FirstOrDefaultAsync(t => t.TemplateType == templateType && t.IsActive);

        if (template == null) {
            template = await SeedTemplateAsync(templateType);
        }

        return template;
    }

    private async Task<EmailTemplate> SeedTemplateAsync(string templateType) {
        var templatePath = Path.Combine(
            AppContext.BaseDirectory, 
            "Templates", 
            "Email", 
            $"{templateType}.liquid");

        var body = File.Exists(templatePath) 
            ? await File.ReadAllTextAsync(templatePath)
            : "<p>Alert triggered for operation: {{ operation }}</p>";

        var template = new EmailTemplate {
            Id = Guid.NewGuid(),
            Name = templateType,
            TemplateType = templateType,
            Subject = GetDefaultSubject(templateType),
            Body = body,
            IsActive = true,
            IsSystemTemplate = true
        };

        _dbContext.GetEntitySet<EmailTemplate>().Add(template);
        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Seeded email template for type {TemplateType}", templateType);
        return template;
    }

    private async Task<AlertTemplate> RenderTemplateAsync(
        EmailTemplate emailTemplate, 
        Dictionary<string, object> auditData, 
        string tenantId, 
        string? alertComment) {
        try {
            if (!_parser.TryParse(emailTemplate.Body, out var fluidTemplate, out var error)) {
                _logger.LogError("Failed to parse template {TemplateName}: {Error}", 
                    emailTemplate.Name, error);
                throw new InvalidOperationException($"Template parsing failed: {error}");
            }

            var cippUrl = _configuration["CIPP:BaseUrl"] ?? "https://localhost";
            var userId = auditData.TryGetValue("UserId", out var user) ? user.ToString() : "";
            var operation = auditData.TryGetValue("Operation", out var op) ? op.ToString() : "";

            var context = new TemplateContext(auditData, _templateOptions);
            context.SetValue("user_id", userId);
            context.SetValue("operation", operation);
            context.SetValue("tenant_id", tenantId);
            context.SetValue("cipp_url", cippUrl);
            context.SetValue("audit_data", ConvertDictToFluidArray(auditData));

            var content = await fluidTemplate.RenderAsync(context);

            var title = emailTemplate.TemplateType switch {
                "NewInboxRule" => $"{tenantId} - New Rule Detected for {userId}",
                "SetInboxRule" => $"{tenantId} - Rule Edit Detected for {userId}",
                "RoleChange" => $"{tenantId} - Role change detected",
                "UserLoggedIn" => $"{tenantId} - User logged in from monitored location",
                _ => $"CIPP Alert - {operation}"
            };

            var buttonUrl = GetButtonUrl(emailTemplate.TemplateType, userId ?? "", tenantId, cippUrl);
            var buttonText = GetButtonText(emailTemplate.TemplateType);

            return new AlertTemplate {
                Title = title,
                HtmlContent = content,
                ButtonUrl = buttonUrl,
                ButtonText = buttonText,
                Data = auditData
            };
        } catch (Exception ex) {
            _logger.LogError(ex, "Error rendering template {TemplateName}", emailTemplate.Name);
            throw;
        }
    }

    private static object ConvertDictToFluidArray(Dictionary<string, object> dict) {
        return dict.Select(kvp => new { key = kvp.Key, value = kvp.Value?.ToString() ?? "" }).ToList();
    }

    private static string GetDefaultSubject(string templateType) {
        return templateType switch {
            "NewInboxRule" => "CIPP Alert - New Inbox Rule Detected",
            "SetInboxRule" => "CIPP Alert - Inbox Rule Modified",
            "RoleChange" => "CIPP Alert - Role Assignment Changed",
            "UserLoggedIn" => "CIPP Alert - User Login from Monitored Location",
            _ => "CIPP Alert - Security Event Detected"
        };
    }

    private static string GetButtonUrl(string templateType, string userId, string tenantId, string cippUrl) {
        return templateType switch {
            "NewInboxRule" or "SetInboxRule" or "UserLoggedIn" => 
                $"{cippUrl}/identity/administration/users/user/bec?userId={userId}&tenantFilter={tenantId}",
            "RoleChange" => $"{cippUrl}/identity/administration/roles?customerId={tenantId}",
            _ => $"{cippUrl}/cipp/logs"
        };
    }

    private static string GetButtonText(string templateType) {
        return templateType switch {
            "NewInboxRule" or "SetInboxRule" or "UserLoggedIn" => "Start BEC Investigation",
            "RoleChange" => "Role Management",
            _ => "Check Logbook"
        };
    }

    private string GetBaseTemplate() {
        var baseTemplatePath = Path.Combine(
            AppContext.BaseDirectory, 
            "Templates", 
            "Email", 
            "BaseTemplate.liquid");

        return File.Exists(baseTemplatePath) 
            ? File.ReadAllText(baseTemplatePath)
            : "<!DOCTYPE html><html><body>{{ content }}</body></html>";
    }

    private static string GenerateFallbackHtml(AlertTemplate template, string? alertComment) {
        var html = $"<html><body><h2>{template.Title}</h2><div>{template.HtmlContent}</div>";
        if (!string.IsNullOrEmpty(alertComment)) {
            html += $"<div style='background:#f0f0f0;padding:10px;margin:10px 0;'>" +
                   $"<strong>Alert Info:</strong> {alertComment}</div>";
        }
        html += "</body></html>";
        return html;
    }

    public async Task<string> GenerateAlertHtmlAsync(
        Dictionary<string, object> enrichedData,
        string tenantId,
        string operation,
        string? alertComment = null) {
        var alertTemplate = await GenerateAuditLogTemplateAsync(enrichedData, tenantId, alertComment);
        return GenerateHtmlEmail(alertTemplate, alertComment);
    }

    public async Task<Dictionary<string, object>> GenerateAlertJsonAsync(
        Dictionary<string, object> enrichedData,
        string tenantId,
        string operation,
        string? alertComment = null) {
        var alertTemplate = await GenerateAuditLogTemplateAsync(enrichedData, tenantId, alertComment);
        var jsonString = GenerateJsonPayload(alertTemplate, alertComment);
        return JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString) ?? new Dictionary<string, object>();
    }
}
