using McpClient.SonarReportGenerator.Models;
using McpClient.SonarReportGenerator.Models.AiPlatform.Contracts;
using McpClient.SonarReportGenerator.Models.AiPlatform.Contractss;
using System.Text.Json;

namespace McpClient.Services
{
    public class SonarReportOrchestrator
    {        
        private readonly DebtAgent _debtAgent;
        private readonly RecommendationAgent _recommendationAgent;     
       
        private readonly HtmlRenderer _htmlRenderer;

        public SonarReportOrchestrator(
            RecommendationAgent recommendationAgent,
            DebtAgent debtAgent,
            HtmlRenderer htmlRenderer)
        {
            _recommendationAgent = recommendationAgent;
            _htmlRenderer = htmlRenderer;
            _debtAgent = debtAgent;            
        }
        

        public async Task<string> GenerateReportAsync(string sonarJson)
        {
            var report = JsonSerializer.Deserialize<SonarReport>(
                sonarJson,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })
                ?? throw new Exception("Unable to deserialize SonarReport.");
          
            // Everything below is pure C#
            var summary = BuildExecutiveSummary(report);

            var issues = BuildIssuesReport(report);

            var debt = BuildDebtReport(report);

            var recommendationInput =
                BuildRecommendationInput(summary, issues, debt,report.Summary.TotalIssues);

            var debtTask = _debtAgent.Execute(debt);
            var recommendationTask = _recommendationAgent.Execute(recommendationInput);

            await Task.WhenAll(debtTask, recommendationTask);

            var finalReport = new FinalReport
            {
                ExecutiveSummary = summary,
                Issues = issues,
                TechnicalDebt = await debtTask,
                Recommendations = await recommendationTask
            };

            // Pure HTML rendering
            
            return  _htmlRenderer.Render(finalReport);
        }

        private RecommendationInput BuildRecommendationInput(
    ExecutiveSummary summary,
    IssuesReport issues,
    DebtReport debt,
            int totalIssue)
        {
            return new RecommendationInput
            {
                ProjectName = summary.ProjectName,

                QualityGate = summary.QualityGate,

                Coverage = summary.Coverage,

                CriticalIssues =
                    summary.SeveritySummary.Critical,

                BlockerIssues =
                    summary.SeveritySummary.Blocker,

                Vulnerabilities =
                    totalIssue,

                TopDebtFiles =
                    debt.DebtFiles
                        .Take(5)
                        .Select(x => x.FileName)
                        .ToList(),

                TopIssueFiles =
                    issues.TopFiles
                        .Take(5)
                        .Select(x => x.FileName)
                        .ToList()
            };
        }
        private ExecutiveSummary BuildExecutiveSummary(SonarReport report)
        {
            return new ExecutiveSummary
            {
                ProjectName = report.ProjectKey,

                QualityGate = report.Summary.QualityGate,

                Reliability = report.Summary.Reliability,

                Security = report.Summary.Security,

                Maintainability = report.Summary.Maintainability,

                Coverage = report.Summary.Coverage.ToString("F2"),

                Duplication = report.Summary.Duplication.ToString("F2"),
                Bugs = report.Summary.Bugs,
                Vulnerabilities = report.Summary.Vulnerabilities,
                CodeSmells = report.Summary.CodeSmells,
                TotalIssues = report.Summary.TotalIssues,
                SeveritySummary = new SeveritySummary
                {
                    Blocker = report.Summary.Blocker,
                    Critical = report.Summary.Critical,
                    Major = report.Summary.Major,
                    Minor = report.Summary.Minor,
                    Info = report.Summary.Info
                },

                ExecutiveSummaryText =
                    BuildExecutiveSummaryText(report),

                QualityOverview =
                    BuildQualityOverview(report)
            };
        }
        private string BuildExecutiveSummaryText(SonarReport report)
        {
            return
        $"""
Project **{report.ProjectKey}** has a **{report.Summary.QualityGate}** Quality Gate.

The project contains **{report.Summary.Blocker} blocker**, **{report.Summary.Critical} critical** and **{report.Summary.Major} major** issues.

Current code coverage is **{report.Summary.Coverage:F2}%** with **{report.Summary.Duplication:F2}%** duplicated code.
""";
        }
        private string BuildQualityOverview(SonarReport report)
        {
            return
        $"""
| Metric | Status |
|--------|--------|
| Reliability | {report.Summary.Reliability} |
| Security | {report.Summary.Security} |
| Maintainability | {report.Summary.Maintainability} |
| Coverage | {report.Summary.Coverage:F2}% |
| Duplication | {report.Summary.Duplication:F2}% |
""";
        }
      
        private DebtReport BuildDebtReport(SonarReport report)
        {
            return new DebtReport
            {
                TotalDebtFile=report.Files.Count,
                DebtFiles = report.Files
                    .OrderByDescending(f => f.TotalIssues)
                    .Take(20)
                    .ToList()
            };
        }


        private IssuesReport BuildIssuesReport(SonarReport report)
        {
            return new IssuesReport
            {
                TopFiles = PopulateLanguageForFiles(
                    report.Files
                        .OrderByDescending(f => f.TotalIssues)
                        .Take(20)
                        .ToList(),
                    report.Issues),

                TopIssues = report.Issues
                    .OrderByDescending(i => SeverityWeight(i.Severity))
                    .Take(20)
                    .ToList()
            };
        }
      
        private List<FileSummary> PopulateLanguageForFiles(List<FileSummary> files, List<SonarIssue> issues)
        {
            foreach (var file in files)
            {
                if (string.IsNullOrWhiteSpace(file.Language))
                {
                    file.Language = GetFallbackLanguage(file.FileName);
                }
                var fileIssues = issues
            .Where(i => i.Component == file.FileName)
            .ToList();

                file.Summary = BuildFileSummary(fileIssues);
            }
            return files;
        }
        private string BuildFileSummary(List<SonarIssue> issues)
        {
            if (!issues.Any())
                return "No issues found.";

            var summary = issues
                .GroupBy(i => i.Type)
                .Select(g => $"{g.Count()} {g.Key}");

            return string.Join(", ", summary);
        }
        private static string GetFallbackLanguage(string fileName)
        {
            var extension = Path.GetExtension(fileName)
                .ToLowerInvariant();

            return extension switch
            {
                ".cs" => "C#",
                ".js" => "JavaScript",
                ".ts" => "TypeScript",
                ".jsx" => "JavaScript",
                ".tsx" => "TypeScript",

                ".html" => "HTML",
                ".htm" => "HTML",
                ".aspx" => "ASP.NET",
                ".ascx" => "ASP.NET",
                ".cshtml" => "Razor",
                ".razor" => "Razor",

                ".css" => "CSS",
                ".scss" => "SCSS",
                ".less" => "LESS",

                ".xml" => "XML",
                ".config" => "XML",

                ".json" => "JSON",
                ".yaml" => "YAML",
                ".yml" => "YAML",

                ".sql" => "SQL",

                ".java" => "Java",
                ".py" => "Python",
                ".go" => "Go",
                ".rs" => "Rust",
                ".cpp" => "C++",
                ".cc" => "C++",
                ".c" => "C",
                ".h" => "C/C++",

                ".ps1" => "PowerShell",
                ".sh" => "Shell",
                ".bat" => "Batch",

                ".md" => "Markdown",

                _ => "Unknown"
            };
        }
        private static int SeverityWeight(string severity)
        {
            return severity.ToUpperInvariant() switch
            {
                "BLOCKER" => 5,
                "CRITICAL" => 4,
                "MAJOR" => 3,
                "MINOR" => 2,
                "INFO" => 1,
                _ => 0
            };
        }
       
    }
}
