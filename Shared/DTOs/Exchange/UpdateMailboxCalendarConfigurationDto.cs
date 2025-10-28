namespace CIPP.Shared.DTOs.Exchange;

public class UpdateMailboxCalendarConfigurationDto {
    public bool? AutomateProcessing { get; set; }
    public bool? AddOrganizerToSubject { get; set; }
    public bool? DeleteComments { get; set; }
    public bool? DeleteSubject { get; set; }
    public bool? RemovePrivateProperty { get; set; }
    public int? WorkDays { get; set; }
    public string? WorkingHoursStartTime { get; set; }
    public string? WorkingHoursEndTime { get; set; }
    public string? WorkingHoursTimeZone { get; set; }
    public string? WeekStartDay { get; set; }
    public bool? ShowWeekNumbers { get; set; }
    public string? TimeFormat { get; set; }
    public string? DateFormat { get; set; }
    public int? RemindersEnabled { get; set; }
    public int? ReminderSoundEnabled { get; set; }
    public int? DefaultReminderTime { get; set; }
}
