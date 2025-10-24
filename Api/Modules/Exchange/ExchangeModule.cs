using CIPP.Api.Modules.Exchange.Endpoints.CalendarProcessing;
using CIPP.Api.Modules.Exchange.Endpoints.Connectors;
using CIPP.Api.Modules.Exchange.Endpoints.Contacts;
using CIPP.Api.Modules.Exchange.Endpoints.DistributionGroups;
using CIPP.Api.Modules.Exchange.Endpoints.InboxRules;
using CIPP.Api.Modules.Exchange.Endpoints.MailboxAdvanced;
using CIPP.Api.Modules.Exchange.Endpoints.Mailboxes;
using CIPP.Api.Modules.Exchange.Endpoints.MessageTrace;
using CIPP.Api.Modules.Exchange.Endpoints.SafePolicies;
using CIPP.Api.Modules.Exchange.Endpoints.SpamFilters;
using CIPP.Api.Modules.Exchange.Endpoints.TransportRules;
using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Api.Modules.Exchange.Services;
using DispatchR.Extensions;
using System.Reflection;

namespace CIPP.Api.Modules.Exchange;

public class ExchangeModule {
    public void RegisterServices(IServiceCollection services, IConfiguration configuration) {
        services.AddDispatchR(Assembly.GetExecutingAssembly(), withPipelines: true, withNotifications: true);
        services.AddScoped<IMailboxService, MailboxService>();
        services.AddScoped<IContactService, ContactService>();
        services.AddScoped<IExchangeOnlineService, ExchangeOnlineService>();
        services.AddScoped<ITransportRuleService, TransportRuleService>();
        services.AddScoped<ISpamFilterService, SpamFilterService>();
        services.AddScoped<ISafePoliciesService, SafePoliciesService>();
        services.AddScoped<IConnectorService, ConnectorService>();
        services.AddScoped<IDistributionGroupService, DistributionGroupService>();
        services.AddScoped<ICalendarProcessingService, CalendarProcessingService>();
        services.AddScoped<IMessageTraceService, MessageTraceService>();
    }

    public void ConfigureEndpoints(RouteGroupBuilder group) {
        var mailboxesGroup = group.MapGroup("/mailboxes").WithTags("Exchange Mailboxes");
        mailboxesGroup.MapGetMailboxes();
        mailboxesGroup.MapGetMailbox();
        mailboxesGroup.MapUpdateMailbox();
        mailboxesGroup.MapGetMailboxPermissions();
        mailboxesGroup.MapUpdateMailboxPermissions();
        mailboxesGroup.MapGetMailboxForwarding();
        mailboxesGroup.MapUpdateMailboxForwarding();
        mailboxesGroup.MapGetCalendarProcessing();
        mailboxesGroup.MapUpdateCalendarProcessing();
        
        var contactsGroup = group.MapGroup("/contacts").WithTags("Exchange Contacts");
        contactsGroup.MapGetContacts();
        contactsGroup.MapGetContact();
        contactsGroup.MapCreateContact();
        contactsGroup.MapUpdateContact();
        contactsGroup.MapDeleteContact();
        
        var transportRulesGroup = group.MapGroup("/transport-rules").WithTags("Exchange Transport Rules");
        transportRulesGroup.MapGetTransportRules();
        transportRulesGroup.MapGetTransportRule();
        transportRulesGroup.MapCreateTransportRule();
        transportRulesGroup.MapUpdateTransportRule();
        transportRulesGroup.MapDeleteTransportRule();
        transportRulesGroup.MapEnableTransportRule();
        
        var spamFiltersGroup = group.MapGroup("/spam-filters").WithTags("Exchange Spam Filters");
        spamFiltersGroup.MapGetAntiSpamPolicies();
        spamFiltersGroup.MapGetAntiSpamPolicy();
        spamFiltersGroup.MapUpdateAntiSpamPolicy();
        spamFiltersGroup.MapGetAntiMalwarePolicies();
        spamFiltersGroup.MapGetAntiMalwarePolicy();
        spamFiltersGroup.MapUpdateAntiMalwarePolicy();
        spamFiltersGroup.MapGetQuarantineMessages();
        spamFiltersGroup.MapReleaseQuarantineMessage();
        spamFiltersGroup.MapDeleteQuarantineMessage();
        
        var inboxRulesGroup = group.MapGroup("/mailboxes/{mailboxId}/inbox-rules").WithTags("Exchange Inbox Rules");
        inboxRulesGroup.MapGetInboxRules();
        inboxRulesGroup.MapGetInboxRule();
        inboxRulesGroup.MapCreateInboxRule();
        inboxRulesGroup.MapUpdateInboxRule();
        inboxRulesGroup.MapDeleteInboxRule();
        
        var mailboxAdvancedGroup = group.MapGroup("/mailboxes/{mailboxId}").WithTags("Exchange Mailbox Advanced");
        mailboxAdvancedGroup.MapEnableArchive();
        mailboxAdvancedGroup.MapGetMailboxStatistics();
        mailboxAdvancedGroup.MapGetMailboxQuota();
        mailboxAdvancedGroup.MapUpdateMailboxQuota();
        mailboxAdvancedGroup.MapUpdateLitigationHold();
        
        var safeLinksGroup = group.MapGroup("/safe-links").WithTags("Exchange Safe Links");
        safeLinksGroup.MapGetSafeLinksPolicies();
        safeLinksGroup.MapGetSafeLinksPolicy();
        safeLinksGroup.MapUpdateSafeLinksPolicy();
        
        var safeAttachmentsGroup = group.MapGroup("/safe-attachments").WithTags("Exchange Safe Attachments");
        safeAttachmentsGroup.MapGetSafeAttachmentPolicies();
        safeAttachmentsGroup.MapGetSafeAttachmentPolicy();
        safeAttachmentsGroup.MapUpdateSafeAttachmentPolicy();
        
        var atpPolicyGroup = group.MapGroup("/atp-policy").WithTags("Exchange ATP Policy");
        atpPolicyGroup.MapGetAtpPolicy();
        atpPolicyGroup.MapUpdateAtpPolicy();
        
        var connectorsGroup = group.MapGroup("/connectors").WithTags("Exchange Connectors");
        connectorsGroup.MapGetAllConnectors();
        connectorsGroup.MapGetInboundConnector();
        connectorsGroup.MapGetOutboundConnector();
        connectorsGroup.MapCreateInboundConnector();
        connectorsGroup.MapCreateOutboundConnector();
        connectorsGroup.MapUpdateInboundConnector();
        connectorsGroup.MapUpdateOutboundConnector();
        
        var distributionGroupsGroup = group.MapGroup("/distribution-groups").WithTags("Exchange Distribution Groups");
        distributionGroupsGroup.MapGetDistributionGroups();
        distributionGroupsGroup.MapGetDistributionGroup();
        distributionGroupsGroup.MapCreateDistributionGroup();
        distributionGroupsGroup.MapUpdateDistributionGroup();
        distributionGroupsGroup.MapDeleteDistributionGroup();
        distributionGroupsGroup.MapGetDistributionGroupMembers();
        distributionGroupsGroup.MapAddDistributionGroupMember();
        distributionGroupsGroup.MapRemoveDistributionGroupMember();
        
        var messageTraceGroup = group.MapGroup("/message-trace").WithTags("Exchange Message Trace");
        messageTraceGroup.MapGetMessageTrace();
        messageTraceGroup.MapGetMessageTraceDetail();
    }
}
