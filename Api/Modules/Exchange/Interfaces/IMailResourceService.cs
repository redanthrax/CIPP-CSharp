using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Interfaces;

public interface IMailResourceService {
    Task<PagedResponse<RoomMailboxDto>> GetRoomMailboxesAsync(Guid tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default);
    Task<PagedResponse<EquipmentMailboxDto>> GetEquipmentMailboxesAsync(Guid tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default);
    Task<RoomMailboxDto?> GetRoomMailboxAsync(Guid tenantId, string identity, CancellationToken cancellationToken = default);
    Task<EquipmentMailboxDto?> GetEquipmentMailboxAsync(Guid tenantId, string identity, CancellationToken cancellationToken = default);
    Task<string> CreateRoomMailboxAsync(Guid tenantId, CreateRoomMailboxDto createDto, CancellationToken cancellationToken = default);
    Task<string> CreateEquipmentMailboxAsync(Guid tenantId, CreateEquipmentMailboxDto createDto, CancellationToken cancellationToken = default);
    Task UpdateResourceBookingPolicyAsync(Guid tenantId, string identity, UpdateResourceBookingPolicyDto updateDto, CancellationToken cancellationToken = default);
}
