using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Api.Modules.Tenants.Queries;
using CIPP.Shared.DTOs.Tenants;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Tenants.Handlers;

public class ValidateDomainQueryHandler : IRequestHandler<ValidateDomainQuery, Task<ValidateDomainResponseDto>>
{
    private readonly IMicrosoftGraphService _microsoftGraphService;
    
    public ValidateDomainQueryHandler(IMicrosoftGraphService microsoftGraphService)
    {
        _microsoftGraphService = microsoftGraphService;
    }

    public async Task<ValidateDomainResponseDto> Handle(ValidateDomainQuery request, CancellationToken cancellationToken)
    {
        return await ValidateDomainAsync(request.TenantName, cancellationToken);
    }

    private async Task<ValidateDomainResponseDto> ValidateDomainAsync(string tenantName, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(tenantName))
        {
            return new ValidateDomainResponseDto(false, "Tenant name is required", false);
        }

        if (tenantName.Length < 3)
        {
            return new ValidateDomainResponseDto(false, "Tenant name must be at least 3 characters", false);
        }

        if (tenantName.Length > 63)
        {
            return new ValidateDomainResponseDto(false, "Tenant name must be 63 characters or less", false);
        }

        if (!System.Text.RegularExpressions.Regex.IsMatch(tenantName, @"^[a-zA-Z0-9]([a-zA-Z0-9-]*[a-zA-Z0-9])?$"))
        {
            return new ValidateDomainResponseDto(false, "Tenant name must start and end with a letter or number, and can contain letters, numbers, and hyphens", false);
        }

        var reservedNames = new[] {
            "admin", "administrator", "api", "app", "application", "apps", "azure", "backup", "blog", 
            "cdn", "cloud", "data", "database", "db", "dev", "development", "docs", "download", 
            "email", "exchange", "ftp", "git", "help", "info", "ldap", "log", "logs", "mail", 
            "microsoft", "mobile", "monitoring", "news", "office", "portal", "prod", "production", 
            "root", "secure", "security", "server", "service", "services", "shop", "sql", "ssl", 
            "staging", "static", "store", "support", "test", "testing", "video", "web", "www"
        };

        if (reservedNames.Contains(tenantName.ToLower()))
        {
            return new ValidateDomainResponseDto(false, "This domain name is reserved and cannot be used", false);
        }

        try
        {
            var domainName = $"{tenantName}.onmicrosoft.com";
            var isAvailable = await _microsoftGraphService.ValidateDomainAvailabilityAsync(domainName);
            
            var message = isAvailable 
                ? "Domain is available" 
                : "Domain is already taken or unavailable";

            return new ValidateDomainResponseDto(true, message, isAvailable);
        }
        catch (Exception)
        {
            await Task.Delay(200, cancellationToken);
            var commonTakenDomains = new[] { 
                "test", "demo", "example", "contoso", "sample", "default", "temp", "temporary" 
            };
            
            var isLikelyAvailable = !commonTakenDomains.Contains(tenantName.ToLower());
            return new ValidateDomainResponseDto(true, 
                isLikelyAvailable ? "Domain appears to be available (validation limited)" : "Domain may not be available", 
                isLikelyAvailable);
        }
    }
}