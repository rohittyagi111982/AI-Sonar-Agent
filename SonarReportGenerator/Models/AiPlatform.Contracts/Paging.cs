using System.Text.Json.Serialization;

namespace McpClient.SonarReportGenerator.Models.AiPlatform.Contracts;

public class Paging
{
    [JsonPropertyName("pageIndex")]
    public int PageIndex { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }
}