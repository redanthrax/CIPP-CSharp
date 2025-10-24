using CIPP.Shared.DTOs.Exchange;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Exchange.Queries;

public record GetSafeAttachmentPolicyQuery(string TenantId, string PolicyName) : IRequest<GetSafeAttachmentPolicyQuery, Task<SafeAttachmentsPolicyDto?>>;
