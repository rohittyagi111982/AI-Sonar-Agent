namespace McpClient.SonarReportGenerator.Models.AiPlatform.Contractss;

public class SonarSummary
{
    public int TotalIssues { get; set; }

    public int Bugs { get; set; }

    public int Vulnerabilities { get; set; }

    public int CodeSmells { get; set; }

    public int Blocker { get; set; }

    public int Critical { get; set; }

    public int Major { get; set; }

    public int Minor { get; set; }

    public int Info { get; set; }
    // New
    public string QualityGate { get; set; } = "";

    public double Coverage { get; set; }

    public double Duplication { get; set; }

    public string Reliability { get; set; } = "";

    public string Security { get; set; } = "";

    public string Maintainability { get; set; } = "";
}