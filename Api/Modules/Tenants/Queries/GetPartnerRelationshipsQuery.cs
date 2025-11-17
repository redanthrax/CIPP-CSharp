using CIPP.Shared.DTOs.Tenants;
using DispatchR.Abstractions.Send;

namespace CIPP.Api.Modules.Tenants.Queries;

public record GetPartnerRelationshipsQuery(Guid TenantId)
    : IRequest<GetPartnerRelationshipsQuery, Task<GraphResultsDto<PartnerRelationshipDto>?>>;
