using System.Text.Json.Serialization;

namespace MCPServerAPI.Models;

public class SonarIssue
{
    [JsonPropertyName("key")]
    public string Key { get; set; } = string.Empty;

    [JsonPropertyName("rule")]
    public string Rule { get; set; } = string.Empty;

    [JsonPropertyName("severity")]
    public string Severity { get; set; } = string.Empty;

    [JsonPropertyName("component")]
    public string Component { get; set; } = string.Empty;

    [JsonPropertyName("project")]
    public string Project { get; set; } = string.Empty;

    [JsonPropertyName("line")]
    public int? Line { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("creationDate")]
    [JsonConverter(typeof(SonarDateTimeConverter))]
    public DateTime CreationDate { get; set; }

    [JsonPropertyName("updateDate")]
    [JsonConverter(typeof(SonarDateTimeConverter))]
    public DateTime UpdateDate { get; set; }  

    [JsonPropertyName("language")]
    public string Language { get; set; } = string.Empty;

}