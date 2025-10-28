namespace CIPP.Shared.DTOs.Exchange;

public class UpdateResourceBookingPolicyDto {
    public bool? AutomateProcessing { get; set; }
    public bool? AllowConflicts { get; set; }
    public bool? AllowRecurringMeetings { get; set; }
    public int? BookingWindowInDays { get; set; }
    public int? MaximumDurationInMinutes { get; set; }
    public string[]? ResourceDelegates { get; set; }
    public string[]? BookInPolicy { get; set; }
    public string[]? RequestInPolicy { get; set; }
    public string[]? RequestOutOfPolicy { get; set; }
    public bool? EnforceSchedulingHorizon { get; set; }
    public bool? ProcessExternalMeetingMessages { get; set; }
}
