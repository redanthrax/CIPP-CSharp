namespace CIPP.Shared.DTOs.Tenants;

public class GraphResultsDto<T> {
    public List<T> Results { get; set; } = new();
}
