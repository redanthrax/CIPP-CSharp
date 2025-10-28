using CIPP.Api.Extensions;
using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Services;

public class JournalRuleService : IJournalRuleService {
    private readonly IExchangeOnlineService _exoService;
    private readonly ILogger<JournalRuleService> _logger;

    public JournalRuleService(IExchangeOnlineService exoService, ILogger<JournalRuleService> logger) {
        _exoService = exoService;
        _logger = logger;
    }

    public async Task<PagedResponse<JournalRuleDto>> GetJournalRulesAsync(Guid tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting journal rules for tenant {TenantId}", tenantId);
        
        var rules = await _exoService.ExecuteCmdletListAsync<JournalRuleDto>(
            tenantId,
            "Get-JournalRule",
            null,
            cancellationToken
        );

        foreach (var rule in rules) {
            rule.TenantId = tenantId;
        }

        return rules.ToPagedResponse(pagingParams);
    }

    public async Task<string> CreateJournalRuleAsync(Guid tenantId, CreateJournalRuleDto createDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Creating journal rule {RuleName} for tenant {TenantId}", createDto.Name, tenantId);
        
        var parameters = BuildCreateParameters(createDto);

        await _exoService.ExecuteCmdletNoResultAsync(
            tenantId,
            "New-JournalRule",
            parameters,
            cancellationToken
        );

        _logger.LogInformation("Successfully created journal rule {RuleName}", createDto.Name);
        return createDto.Name;
    }

    public async Task DeleteJournalRuleAsync(Guid tenantId, string ruleName, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Deleting journal rule {RuleName} for tenant {TenantId}", ruleName, tenantId);
        
        var parameters = new Dictionary<string, object> {
            { "Identity", ruleName },
            { "Confirm", false }
        };

        await _exoService.ExecuteCmdletNoResultAsync(
            tenantId,
            "Remove-JournalRule",
            parameters,
            cancellationToken
        );

        _logger.LogInformation("Successfully deleted journal rule {RuleName}", ruleName);
    }

    private Dictionary<string, object> BuildCreateParameters(CreateJournalRuleDto createDto) {
        var parameters = new Dictionary<string, object> {
            { "Name", createDto.Name },
            { "JournalEmailAddress", createDto.JournalEmailAddress },
            { "Scope", createDto.Scope },
            { "Enabled", createDto.Enabled },
            { "LawfulInterception", createDto.LawfulInterception },
            { "FullReport", createDto.FullReport }
        };

        if (!string.IsNullOrEmpty(createDto.Recipient)) {
            parameters.Add("Recipient", createDto.Recipient);
        }

        return parameters;
    }
}
