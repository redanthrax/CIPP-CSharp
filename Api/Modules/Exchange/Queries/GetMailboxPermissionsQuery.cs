using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries;

public record GetMailboxPermissionsQuery(string TenantId, string UserId) : IRequest<GetMailboxPermissionsQuery, Task<List<MailboxPermissionDto>>>;
