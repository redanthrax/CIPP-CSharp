using CIPP.Api.Extensions;
using CIPP.Api.Modules.Exchange.Interfaces;
using CIPP.Shared.DTOs;
using CIPP.Shared.DTOs.Exchange;

namespace CIPP.Api.Modules.Exchange.Services;

public class MailResourceService : IMailResourceService {
    private readonly IExchangeOnlineService _exchangeOnlineService;

    public MailResourceService(IExchangeOnlineService exchangeOnlineService) {
        _exchangeOnlineService = exchangeOnlineService;
    }

    public async Task<PagedResponse<RoomMailboxDto>> GetRoomMailboxesAsync(Guid tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default) {
        var parameters = new Dictionary<string, object> {
            { "RecipientTypeDetails", "RoomMailbox" },
            { "ResultSize", pagingParams.PageSize }
        };

        var mailboxes = await _exchangeOnlineService.ExecuteCmdletListAsync<RoomMailboxDto>(tenantId, "Get-Mailbox", parameters, cancellationToken);

        return mailboxes.ToPagedResponse(pagingParams);
    }

    public async Task<PagedResponse<EquipmentMailboxDto>> GetEquipmentMailboxesAsync(Guid tenantId, PagingParameters pagingParams, CancellationToken cancellationToken = default) {
        var parameters = new Dictionary<string, object> {
            { "RecipientTypeDetails", "EquipmentMailbox" },
            { "ResultSize", pagingParams.PageSize }
        };

        var mailboxes = await _exchangeOnlineService.ExecuteCmdletListAsync<EquipmentMailboxDto>(tenantId, "Get-Mailbox", parameters, cancellationToken);

        return mailboxes.ToPagedResponse(pagingParams);
    }

    public async Task<RoomMailboxDto?> GetRoomMailboxAsync(Guid tenantId, string identity, CancellationToken cancellationToken = default) {
        var parameters = new Dictionary<string, object> {
            { "Identity", identity },
            { "RecipientTypeDetails", "RoomMailbox" }
        };

        return await _exchangeOnlineService.ExecuteCmdletAsync<RoomMailboxDto>(tenantId, "Get-Mailbox", parameters, cancellationToken);
    }

    public async Task<EquipmentMailboxDto?> GetEquipmentMailboxAsync(Guid tenantId, string identity, CancellationToken cancellationToken = default) {
        var parameters = new Dictionary<string, object> {
            { "Identity", identity },
            { "RecipientTypeDetails", "EquipmentMailbox" }
        };

        return await _exchangeOnlineService.ExecuteCmdletAsync<EquipmentMailboxDto>(tenantId, "Get-Mailbox", parameters, cancellationToken);
    }

    public async Task<string> CreateRoomMailboxAsync(Guid tenantId, CreateRoomMailboxDto createDto, CancellationToken cancellationToken = default) {
        var parameters = new Dictionary<string, object> {
            { "Name", createDto.Name },
            { "DisplayName", createDto.DisplayName },
            { "Alias", createDto.Alias },
            { "Room", true }
        };

        if (!string.IsNullOrEmpty(createDto.ResourceCapacity)) {
            parameters.Add("ResourceCapacity", createDto.ResourceCapacity);
        }
        if (createDto.EnableRoomMailboxAccount) {
            parameters.Add("EnableRoomMailboxAccount", true);
            if (!string.IsNullOrEmpty(createDto.RoomMailboxPassword)) {
                parameters.Add("RoomMailboxPassword", createDto.RoomMailboxPassword);
            }
        }

        await _exchangeOnlineService.ExecuteCmdletNoResultAsync(tenantId, "New-Mailbox", parameters, cancellationToken);

        return createDto.Alias;
    }

    public async Task<string> CreateEquipmentMailboxAsync(Guid tenantId, CreateEquipmentMailboxDto createDto, CancellationToken cancellationToken = default) {
        var parameters = new Dictionary<string, object> {
            { "Name", createDto.Name },
            { "DisplayName", createDto.DisplayName },
            { "Alias", createDto.Alias },
            { "Equipment", true }
        };

        if (!string.IsNullOrEmpty(createDto.ResourceCapacity)) {
            parameters.Add("ResourceCapacity", createDto.ResourceCapacity);
        }

        await _exchangeOnlineService.ExecuteCmdletNoResultAsync(tenantId, "New-Mailbox", parameters, cancellationToken);

        return createDto.Alias;
    }

    public async Task UpdateResourceBookingPolicyAsync(Guid tenantId, string identity, UpdateResourceBookingPolicyDto updateDto, CancellationToken cancellationToken = default) {
        var parameters = new Dictionary<string, object> {
            { "Identity", identity }
        };

        if (updateDto.AutomateProcessing.HasValue) {
            parameters["AutomateProcessing"] = updateDto.AutomateProcessing.Value ? "AutoAccept" : "None";
        }
        if (updateDto.AllowConflicts.HasValue) {
            parameters["AllowConflicts"] = updateDto.AllowConflicts.Value;
        }
        if (updateDto.AllowRecurringMeetings.HasValue) {
            parameters["AllowRecurringMeetings"] = updateDto.AllowRecurringMeetings.Value;
        }
        if (updateDto.BookingWindowInDays.HasValue) {
            parameters["BookingWindowInDays"] = updateDto.BookingWindowInDays.Value;
        }
        if (updateDto.MaximumDurationInMinutes.HasValue) {
            parameters["MaximumDurationInMinutes"] = updateDto.MaximumDurationInMinutes.Value;
        }
        if (updateDto.ResourceDelegates != null) {
            parameters["ResourceDelegates"] = updateDto.ResourceDelegates;
        }
        if (updateDto.BookInPolicy != null) {
            parameters["BookInPolicy"] = updateDto.BookInPolicy;
        }
        if (updateDto.RequestInPolicy != null) {
            parameters["RequestInPolicy"] = updateDto.RequestInPolicy;
        }
        if (updateDto.RequestOutOfPolicy != null) {
            parameters["RequestOutOfPolicy"] = updateDto.RequestOutOfPolicy;
        }
        if (updateDto.EnforceSchedulingHorizon.HasValue) {
            parameters["EnforceSchedulingHorizon"] = updateDto.EnforceSchedulingHorizon.Value;
        }
        if (updateDto.ProcessExternalMeetingMessages.HasValue) {
            parameters["ProcessExternalMeetingMessages"] = updateDto.ProcessExternalMeetingMessages.Value;
        }

        await _exchangeOnlineService.ExecuteCmdletNoResultAsync(tenantId, "Set-CalendarProcessing", parameters, cancellationToken);
    }
}
