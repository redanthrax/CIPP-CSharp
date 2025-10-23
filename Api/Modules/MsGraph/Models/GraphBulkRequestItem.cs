namespace CIPP.Api.Modules.MsGraph.Models;

public class GraphBulkRequestItem {
    public string Id { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Method { get; set; } = "GET";
    public string? Body { get; set; }
    public Dictionary<string, string> Headers { get; set; } = new();
}