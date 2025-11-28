using CIPP.Api.Modules.Alerts.Interfaces;
using CIPP.Api.Modules.MsGraph.Interfaces;
using Microsoft.Graph.Beta.Models;

namespace CIPP.Api.Modules.Alerts.Services;

public class EmailAlertActionHandler : IAlertActionHandler {
    private readonly IAlertTemplateService _templateService;
    private readonly IMicrosoftGraphService _graphService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailAlertActionHandler> _logger;

    public string ActionType => "email";

    public EmailAlertActionHandler(
        IAlertTemplateService templateService,
        IMicrosoftGraphService graphService,
        IConfiguration configuration,
        ILogger<EmailAlertActionHandler> logger) {
        _templateService = templateService;
        _graphService = graphService;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task ExecuteAsync(
        Dictionary<string, object> enrichedData,
        string tenantId,
        string? alertComment = null,
        Dictionary<string, string>? additionalParams = null,
        CancellationToken cancellationToken = default) {
        try {
            var recipients = additionalParams?.GetValueOrDefault("recipients")?.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (recipients == null || recipients.Length == 0) {
                _logger.LogWarning("No recipients specified for email alert");
                return;
            }

            var operation = enrichedData.TryGetValue("Operation", out var op) ? op.ToString() : "Generic";
            var htmlContent = await _templateService.GenerateAlertHtmlAsync(
                enrichedData,
                tenantId,
                operation ?? "Generic",
                alertComment);

            var subject = additionalParams?.GetValueOrDefault("subject") ?? $"CIPP Alert: {operation}";
            var fromAddress = _configuration["Email:FromAddress"] ?? "alerts@cipp.local";
            var senderTenantId = _configuration["Email:SenderTenantId"];

            if (string.IsNullOrEmpty(senderTenantId)) {
                _logger.LogWarning("Email sender tenant ID not configured, skipping email notification");
                return;
            }

            var graphClient = await _graphService.GetGraphServiceClientAsync(Guid.Parse(senderTenantId));

            var message = new Message {
                Subject = subject,
                Body = new ItemBody {
                    ContentType = BodyType.Html,
                    Content = htmlContent
                },
                ToRecipients = recipients
                    .Select(r => r.Trim())
                    .Where(r => !string.IsNullOrEmpty(r))
                    .Select(r => new Recipient {
                        EmailAddress = new EmailAddress {
                            Address = r
                        }
                    })
                    .ToList()
            };

            if (message.ToRecipients.Count == 0) {
                _logger.LogWarning("No valid recipients after parsing");
                return;
            }

            await graphClient.Users[fromAddress].SendMail
                .PostAsync(new Microsoft.Graph.Beta.Users.Item.SendMail.SendMailPostRequestBody {
                    Message = message,
                    SaveToSentItems = false
                }, cancellationToken: cancellationToken);

            _logger.LogInformation(
                "Email alert sent successfully to {RecipientCount} recipients for tenant {TenantId}",
                message.ToRecipients.Count, tenantId);
        } catch (Exception ex) {
            _logger.LogError(ex, "Failed to send email alert for tenant {TenantId}", tenantId);
            throw;
        }
    }
}
