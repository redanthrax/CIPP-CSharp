namespace CIPP.Shared.DTOs.Exchange;

public class UpdateCalendarProcessingDto {
    public bool? AutomateProcessing { get; set; }
    public bool? AddAdditionalResponse { get; set; }
    public string? AdditionalResponse { get; set; }
    public bool? AddOrganizerToSubject { get; set; }
    public bool? AllowConflicts { get; set; }
    public bool? AllowRecurringMeetings { get; set; }
    public int? BookingWindowInDays { get; set; }
    public int? MaximumDurationInMinutes { get; set; }
    public int? ConflictPercentageAllowed { get; set; }
    public int? MaximumConflictInstances { get; set; }
    public bool? ForwardRequestsToDelegates { get; set; }
    public bool? DeleteAttachments { get; set; }
    public bool? DeleteComments { get; set; }
    public bool? DeleteSubject { get; set; }
    public bool? RemovePrivateProperty { get; set; }
    public bool? ProcessExternalMeetingMessages { get; set; }
    public bool? RemoveForwardedMeetingNotifications { get; set; }
    public List<string>? ResourceDelegates { get; set; }
    public List<string>? RequestOutOfPolicy { get; set; }
    public List<string>? BookInPolicy { get; set; }
    public List<string>? RequestInPolicy { get; set; }
    public bool? AddNewRequestsTentatively { get; set; }
    public bool? OrganizerInfo { get; set; }
    public bool? AllBookInPolicy { get; set; }
    public bool? AllRequestInPolicy { get; set; }
    public bool? AllRequestOutOfPolicy { get; set; }
    public bool? EnforceSchedulingHorizon { get; set; }
    public bool? ScheduleOnlyDuringWorkHours { get; set; }
}
