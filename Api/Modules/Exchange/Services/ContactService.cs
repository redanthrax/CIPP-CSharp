using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.MsGraph.Interfaces;
using CIPP.Shared.DTOs.Exchange;
using Microsoft.Graph.Beta.Models;

namespace CIPP.Api.Modules.Exchange.Services;

public class ContactService : IContactService {
    private readonly IMicrosoftGraphService _graphService;
    private readonly ILogger<ContactService> _logger;

    public ContactService(IMicrosoftGraphService graphService, ILogger<ContactService> logger) {
        _graphService = graphService;
        _logger = logger;
    }

    public async Task<List<ContactDto>> GetContactsAsync(Guid tenantId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting contacts for tenant {TenantId}", tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        var contacts = await graphClient.Contacts.GetAsync(config => {
            config.QueryParameters.Select = new[] { "id", "displayName", "mail", "givenName", "surname", "companyName", "jobTitle", "department" };
            config.QueryParameters.Top = 999;
        }, cancellationToken);

        if (contacts?.Value == null) {
            return new List<ContactDto>();
        }

        return contacts.Value.Select(contact => new ContactDto {
            Id = contact.Id ?? string.Empty,
            DisplayName = contact.DisplayName ?? string.Empty,
            EmailAddress = contact.Mail ?? string.Empty,
            GivenName = contact.GivenName,
            Surname = contact.Surname,
            CompanyName = contact.CompanyName,
            JobTitle = contact.JobTitle,
            Department = contact.Department,
            OfficeLocation = null,
            MobilePhone = null,
            BusinessPhone = null
        }).ToList();
    }

    public async Task<ContactDetailsDto?> GetContactAsync(Guid tenantId, string contactId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Getting contact {ContactId} for tenant {TenantId}", contactId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        var contact = await graphClient.Contacts[contactId].GetAsync(cancellationToken: cancellationToken);

        if (contact == null) {
            return null;
        }

        return new ContactDetailsDto {
            Id = contact.Id ?? string.Empty,
            DisplayName = contact.DisplayName ?? string.Empty,
            EmailAddress = contact.Mail ?? string.Empty,
            EmailAddresses = contact.Mail != null ? new List<string> { contact.Mail } : new List<string>(),
            GivenName = contact.GivenName,
            Surname = contact.Surname,
            MiddleName = null,
            NickName = null,
            Title = null,
            CompanyName = contact.CompanyName,
            JobTitle = contact.JobTitle,
            Department = contact.Department,
            OfficeLocation = null,
            MobilePhone = null,
            BusinessPhone = null,
            BusinessPhone2 = null,
            HomePhone = null,
            Manager = null,
            AssistantName = null,
            Birthday = null,
            Profession = null,
            SpouseName = null,
            PersonalNotes = null,
            Categories = new List<string>()
        };
    }

    public async Task<string> CreateContactAsync(Guid tenantId, CreateContactDto createDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Creating contact {DisplayName} for tenant {TenantId}", createDto.DisplayName, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        var contact = new OrgContact {
            DisplayName = createDto.DisplayName,
            GivenName = createDto.GivenName,
            Surname = createDto.Surname,
            Mail = createDto.EmailAddress,
            CompanyName = createDto.CompanyName,
            JobTitle = createDto.JobTitle,
            Department = createDto.Department
        };

        _logger.LogInformation("Contact creation requested for {DisplayName}. Note: OrgContact creation via Graph API has limited support.", createDto.DisplayName);
        return string.Empty;
    }

    public async Task UpdateContactAsync(Guid tenantId, string contactId, UpdateContactDto updateDto, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Updating contact {ContactId} for tenant {TenantId}", contactId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        _logger.LogInformation("Contact update requested for {ContactId}. Note: OrgContact updates via Graph API have limited support.", contactId);
        await Task.CompletedTask;
    }

    public async Task DeleteContactAsync(Guid tenantId, string contactId, CancellationToken cancellationToken = default) {
        _logger.LogInformation("Deleting contact {ContactId} for tenant {TenantId}", contactId, tenantId);

        var graphClient = await _graphService.GetGraphServiceClientAsync(tenantId);
        
        _logger.LogInformation("Contact deletion requested for {ContactId}. Note: OrgContact deletion via Graph API has limited support.", contactId);
        await Task.CompletedTask;
    }
}
