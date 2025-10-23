using System.Text.Json;

namespace CIPP.Api.Modules.MsGraph.Models;

public class GraphBulkResponse {
    public string Id { get; set; } = string.Empty;
    public int Status { get; set; }
    public JsonElement Body { get; set; }
}