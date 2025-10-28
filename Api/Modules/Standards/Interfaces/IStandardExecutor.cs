using CIPP.Shared.DTOs.Standards;

namespace CIPP.Api.Modules.Standards.Interfaces;

public interface IStandardExecutor {
    string StandardType { get; }
    
    Task<StandardResultDto> ExecuteAsync(Guid tenantId, string configuration, string? executedBy, CancellationToken cancellationToken = default);
    
    Task<bool> ValidateConfigurationAsync(string configuration, CancellationToken cancellationToken = default);
}
