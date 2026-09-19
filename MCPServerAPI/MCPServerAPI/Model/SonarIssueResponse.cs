using System.Text.Json.Serialization;

namespace MCPServerAPI.Models;

public class SonarIssueResponse
{
    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonPropertyName("paging")]
    public Paging Paging { get; set; } = new();

    [JsonPropertyName("issues")]
    public List<SonarIssue> Issues { get; set; } = new();
}