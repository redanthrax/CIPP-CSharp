namespace CIPP.Shared.DTOs.Exchange;

public class LitigationHoldDto {
    public bool LitigationHoldEnabled { get; set; }
    public DateTime? LitigationHoldDate { get; set; }
    public string? LitigationHoldOwner { get; set; }
    public int? LitigationHoldDuration { get; set; }
}
