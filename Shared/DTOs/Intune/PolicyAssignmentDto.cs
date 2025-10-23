namespace CIPP.Shared.DTOs.Intune;

public class PolicyAssignmentDto {
    public string Id { get; set; } = string.Empty;
    public AssignmentTargetDto? Target { get; set; }
}

public class AssignmentTargetDto {
    public string Type { get; set; } = string.Empty;
    public string? GroupId { get; set; }
    public string? DeviceAndAppManagementAssignmentFilterId { get; set; }
    public string? DeviceAndAppManagementAssignmentFilterType { get; set; }
}
