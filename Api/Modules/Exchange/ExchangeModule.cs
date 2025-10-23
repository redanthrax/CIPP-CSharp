using CIPP.Api.Modules.Exchange.Endpoints.Contacts;
using CIPP.Api.Modules.Exchange.Endpoints.Mailboxes;
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
    }
}
