namespace CIPP.Shared.DTOs.Exchange;

public class EquipmentMailboxDto {
    public string Identity { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string PrimarySmtpAddress { get; set; } = string.Empty;
    public string Alias { get; set; } = string.Empty;
    public string ResourceCapacity { get; set; } = string.Empty;
    public bool AutomateProcessing { get; set; }
    public bool AllowConflicts { get; set; }
    public bool AllowRecurringMeetings { get; set; }
    public int BookingWindowInDays { get; set; }
    public int MaximumDurationInMinutes { get; set; }
    public string[] ResourceDelegates { get; set; } = Array.Empty<string>();
    public string[] BookInPolicy { get; set; } = Array.Empty<string>();
    public string[] RequestInPolicy { get; set; } = Array.Empty<string>();
    public string[] RequestOutOfPolicy { get; set; } = Array.Empty<string>();
    public bool EnforceSchedulingHorizon { get; set; }
    public bool ProcessExternalMeetingMessages { get; set; }
}
