namespace McpClient.SonarReportGenerator.Models
{
    public class FinalReport
    {
        public ExecutiveSummary ExecutiveSummary { get; set; } = new();
        public QualityOverview QualityOverview { get; set; } = new();
        public SeveritySummary SeveritySummary { get; set; } = new();
        public IssuesReport Issues { get; set; } = new();
        public DebtReport TechnicalDebt { get; set; } = new();
        public RecommendationReport Recommendations { get; set; } = new();
    }
}
