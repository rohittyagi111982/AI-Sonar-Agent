namespace McpClient.SonarReportGenerator.Models.AiPlatform.Contractss;

public class SonarIssue
{
    public string Key { get; set; }

    public string Rule { get; set; }

    public string Severity { get; set; }

    public string Component { get; set; }

    public string Project { get; set; }

    public int? Line { get; set; }

    public string Message { get; set; }

    public string Type { get; set; }

    public string Status { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime UpdateDate { get; set; }
    public string Language { get; set; } = string.Empty;
}