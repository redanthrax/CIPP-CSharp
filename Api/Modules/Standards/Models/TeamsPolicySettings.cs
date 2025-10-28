namespace CIPP.Api.Modules.Standards.Models;

public class TeamsPolicySettings {
    public bool? AllowGuestAccess { get; set; }
    public bool? AllowPrivateCalling { get; set; }
    public bool? AllowPrivateMeetingScheduling { get; set; }
    public bool? AllowChannelMeetingScheduling { get; set; }
    public Dictionary<string, object>? MeetingPolicies { get; set; }
    public Dictionary<string, object>? MessagingPolicies { get; set; }
}
