using McpClient.SonarReportGenerator.Models;
using System.Text;

public class HtmlRenderer
{
    private readonly string _templateFolder;

    public HtmlRenderer()
    {
        _templateFolder = Path.Combine(AppContext.BaseDirectory, "SonarReportGenerator/Templates");
    }

    public string Render(FinalReport report)
    {
        var html = File.ReadAllText(
            Path.Combine(_templateFolder, "ReportTemplate.html"));

        html = html.Replace("{{ProjectName}}", report.ExecutiveSummary.ProjectName);
        html = html.Replace("{{GeneratedOn}}", DateTime.Now.ToString("dd-MMM-yyyy HH:mm"));
        html = html.Replace("{{QualityGate}}", report.ExecutiveSummary.QualityGate);

        html = html.Replace(
            "{{ExecutiveSummarySection}}",
            RenderSummary(report.ExecutiveSummary));

        html = html.Replace(
            "{{IssuesSection}}",
            RenderIssues(report.Issues, report.ExecutiveSummary));
        html = html.Replace(
            "{{TechnicalDebtSection}}",
            RenderDebt(report.TechnicalDebt));

        html = html.Replace(
            "{{RecommendationSection}}",
            RenderRecommendation(report.Recommendations));

        return html;
    }
    private string RenderRecommendation(
    RecommendationReport recommendation)
    {
        var html = File.ReadAllText(
            Path.Combine(_templateFolder, "recommendations.html"));

        html = html.Replace(
            "{{HighPriorityRecommendations}}",
            BuildRecommendations(recommendation.HighPriority));

        html = html.Replace(
            "{{MediumPriorityRecommendations}}",
            BuildRecommendations(recommendation.MediumPriority));

        html = html.Replace(
            "{{LowPriorityRecommendations}}",
            BuildRecommendations(recommendation.LowPriority));

        html = html.Replace(
            "{{Conclusion}}",
            recommendation.Conclusion);

        return html;
    }
    private string BuildRecommendations(List<string> recommendations)
    {
        if (recommendations == null || !recommendations.Any())
        {
            return "<li>No recommendations available.</li>";
        }

        var sb = new StringBuilder();

        foreach (var recommendation in recommendations)
        {
            sb.AppendLine($@"
  <li>
    {System.Net.WebUtility.HtmlEncode(recommendation)}
  </li>");
        }

        return sb.ToString();
    }
    private string RenderSummary(ExecutiveSummary summary)
    {
        var html = File.ReadAllText(
            Path.Combine(_templateFolder, "summary.html"));

        html = html.Replace("{{QualityGate}}", summary.QualityGate);
        html = html.Replace("{{Coverage}}", summary.Coverage);
        html = html.Replace("{{Reliability}}", summary.Reliability);
        html = html.Replace("{{Security}}", summary.Security);
        html = html.Replace("{{Maintainability}}", summary.Maintainability);
        html = html.Replace("{{Duplication}}", summary.Duplication);

        html = html.Replace("{{Blocker}}", summary.SeveritySummary.Blocker.ToString());
        html = html.Replace("{{Critical}}", summary.SeveritySummary.Critical.ToString());
        html = html.Replace("{{Major}}", summary.SeveritySummary.Major.ToString());
        html = html.Replace("{{Minor}}", summary.SeveritySummary.Minor.ToString());
        html = html.Replace("{{Info}}", summary.SeveritySummary.Info.ToString());
        html = html.Replace("{{QualityOverview}}", summary.QualityOverview.ToString());
        html = html.Replace("{{ExecutiveSummary}}", summary.ExecutiveSummaryText);

        return html;
    }
    private string RenderIssues(IssuesReport report,ExecutiveSummary ExecutiveSummary)
    {
        var html = File.ReadAllText(
            Path.Combine(_templateFolder, "issues.html"));

        html = html.Replace("{{TopFiles}}", BuildTopFiles(report));

        html = html.Replace("{{TopIssues}}", BuildTopIssues(report));

        html = html.Replace("{{TotalIssues}}", ExecutiveSummary.TotalIssues.ToString());
        html = html.Replace("{{Bugs}}", ExecutiveSummary.Bugs.ToString());
        html = html.Replace("{{Vulnerabilities}}", ExecutiveSummary.Vulnerabilities.ToString());
        html = html.Replace("{{CodeSmells}}", ExecutiveSummary.CodeSmells.ToString());

        return html;
    }
    private string BuildTopIssues(IssuesReport report)
    {
        if (report?.TopIssues == null || !report.TopIssues.Any())
        {
            return @"<tr>
                    <td colspan='5'>No issues found.</td>
                 </tr>";
        }

        var sb = new StringBuilder();

        int index = 1;

        foreach (var issue in report.TopIssues)
        {
            sb.AppendLine($@"
<tr>
    <td>{index++}</td>
    <td>{System.Net.WebUtility.HtmlEncode(issue.Severity)}</td>
    <td>{System.Net.WebUtility.HtmlEncode(issue.Type)}</td>
    <td>{System.Net.WebUtility.HtmlEncode(issue.Component)}</td>
    <td>{System.Net.WebUtility.HtmlEncode(issue.Message)}</td>
</tr>");
        }

        return sb.ToString();
    }
    private string BuildTopFiles(IssuesReport report)
    {
        if (report?.TopFiles == null || !report.TopFiles.Any())
        {
            return @"<tr>
                    <td colspan='4'>No files found.</td>
                 </tr>";
        }

        var sb = new StringBuilder();

        int index = 1;

        foreach (var file in report.TopFiles)
        {
            sb.AppendLine($@"
<tr>
    <td>{index++}</td>
    <td>{System.Net.WebUtility.HtmlEncode(file.FileName)}</td>
      <td>{System.Net.WebUtility.HtmlEncode(file.Language)}</td>
    
    <td>{file.TotalIssues}</td>
</tr>");
        }

        return sb.ToString();
    }
    private string RenderDebt(DebtReport debt)
    {
        var html = File.ReadAllText(
            Path.Combine(_templateFolder, "technical-debt.html"));
        html = html.Replace(
            "{{DebtFileCount}}",
            debt.TotalDebtFile.ToString());
        html = html.Replace(
            "{{TechnicalDebtSummary}}",
            debt.TechnicalDebtSummary);

        html = html.Replace(
            "{{DebtFiles}}",
            BuildDebtRows(debt));

        return html;
    }
    private string BuildDebtRows(DebtReport report)
    {
        if (report?.DebtFiles == null || !report.DebtFiles.Any())
        {
            return @"<tr>
                    <td colspan='5' class='text-center'>
                        No technical debt found.
                    </td>
                 </tr>";
        }

        var sb = new StringBuilder();

        int index = 1;

        foreach (var file in report.DebtFiles)
        {
            sb.AppendLine($@"
<tr>
    <td>{index++}</td>
    <td>{System.Net.WebUtility.HtmlEncode(file.FileName)}</td>  
    <td>{System.Net.WebUtility.HtmlEncode(file.Language)}</td>
    <td>{file.TotalIssues}</td>
    <td>{file.Summary}</td>
   
</tr>");
        }

        return sb.ToString();
    }
}