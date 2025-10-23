namespace CIPP.Shared.DTOs.Exchange;

public class MailboxStatisticsDto {
    public long? TotalItemSize { get; set; }
    public int? ItemCount { get; set; }
    public DateTime? LastLogonTime { get; set; }
    public DateTime? LastLogoffTime { get; set; }
}
