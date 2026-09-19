namespace McpClient.SonarReportGenerator.Models.AiPlatform.Contracts
{
    public class FileSummary
    {
        public string FileName { get; set; } = string.Empty;

        public int TotalIssues { get; set; }

        public int Bugs { get; set; }

        public int Vulnerabilities { get; set; }

        public int CodeSmells { get; set; }

        public int Blocker { get; set; }

        public int Critical { get; set; }

        public int Major { get; set; }

        public int Minor { get; set; }
        public string Language { get; set; } = string.Empty;

        public string Summary { get; set; } = string.Empty;
    }
}
