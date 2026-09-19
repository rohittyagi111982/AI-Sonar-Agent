using McpClient.SonarReportGenerator.Models;
using McpClient.SonarReportGenerator.Models.AiPlatform.Contracts;
using OpenAI.Chat;
using System.Text;
using System.Text.Json;

public class HtmlRenderAgent
{
    private readonly ChatClient _client;

    private readonly string _prompt;

    public HtmlRenderAgent(ChatClient client)
    {
        _client = client;

        _prompt = File.ReadAllText(
            Path.Combine(
                AppContext.BaseDirectory,
                "SonarReportGenerator",
    "Agents",
                "HtmlRenderAgent.md"));
    }

    public async Task<string> Execute(
        FinalReport report)
    {
        var reportJson =
            JsonSerializer.Serialize(
                report,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });
        Generate(report.TechnicalDebt, report.Issues,report.Recommendations, report.ExecutiveSummary);
        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(_prompt),

            new UserChatMessage(reportJson)
        };

        var completion =
            await _client.CompleteChatAsync(messages);

        return string.Concat(
            completion.Value.Content
                .Where(x => x.Kind == ChatMessageContentPartKind.Text)
                .Select(x => x.Text));
    }

    public string Generate(
       DebtReport debt,
       IssuesReport issues,
       RecommendationReport recommendations,
       ExecutiveSummary summary)
    {
        var report = File.ReadAllText("Templates/ReportTemplate.html");

        var summaryHtml = File.ReadAllText("Templates/summary.html");
        summaryHtml = summaryHtml.Replace(
            "{{TechnicalDebtSummary}}",
            debt.TechnicalDebtSummary);

        var debtHtml = File.ReadAllText("Templates/technical-debt.html");
        debtHtml = debtHtml.Replace(
            "{{DebtFileRows}}",
            BuildDebtRows(debt.DebtFiles));

        var issuesHtml = RenderIssues(issues);

        var recommendationsHtml = RenderRecommendations(recommendations);

        report = report.Replace("{{SUMMARY}}", summaryHtml);
        report = report.Replace("{{TECHNICAL_DEBT}}", debtHtml);
        report = report.Replace("{{ISSUES}}", issuesHtml);
        report = report.Replace("{{RECOMMENDATIONS}}", recommendationsHtml);

        return report;
    }
    private string BuildDebtRows(List<FileSummary> debtFiles)
    {
        var sb = new StringBuilder();

        foreach (var file in debtFiles)
        {
            sb.AppendLine($@"
<tr>
    <td>{System.Net.WebUtility.HtmlEncode(file.FileName)}</td>
    <td class='text-center'>{file.TotalIssues}</td>
 
</tr>");
        }

        return sb.ToString();
    }
    private string RenderIssues(IssuesReport report)
    {
        var template = File.ReadAllText("Templates/issues.html");

        var sb = new StringBuilder();

        foreach (var file in report.TopFiles)
        {
            sb.AppendLine($@"
            <tr>
                <td>{file.FileName}</td>   
                <td>{file.TotalIssues}</td>
                <td>{file.Bugs}</td>
                <td>{file.Vulnerabilities}</td>
                <td>{file.CodeSmells}</td>
                <td>{file.Critical}</td>
                <td>{file.Major}</td>
                <td>{file.Minor}</td>
            </tr>");
        }

        return template.Replace("{{IssueRows}}", sb.ToString());
    }
    private string RenderRecommendations(RecommendationReport report)
    {
        var template = File.ReadAllText("Templates/recommendations.html");

        var sb = new StringBuilder();

        // Executive Summary
        sb.AppendLine("<div class='card mb-4'>");
        sb.AppendLine("<h3>Executive Summary</h3>");
       // sb.AppendLine($"<p>{report.Summary.AiSummary}</p>");
        sb.AppendLine("</div>");

        // High Priority
        sb.AppendLine("<div class='card mb-4'>");
        sb.AppendLine("<h3 class='text-danger'>High Priority Recommendations</h3>");

        if (report.HighPriority.Any())
        {
            sb.AppendLine("<ul>");

            foreach (var item in report.HighPriority)
                sb.AppendLine($"<li>{item}</li>");

            sb.AppendLine("</ul>");
        }
        else
        {
            sb.AppendLine("<p>No High Priority recommendations.</p>");
        }

        sb.AppendLine("</div>");

        // Medium Priority
        sb.AppendLine("<div class='card mb-4'>");
        sb.AppendLine("<h3 class='text-warning'>Medium Priority Recommendations</h3>");

        if (report.MediumPriority.Any())
        {
            sb.AppendLine("<ul>");

            foreach (var item in report.MediumPriority)
                sb.AppendLine($"<li>{item}</li>");

            sb.AppendLine("</ul>");
        }
        else
        {
            sb.AppendLine("<p>No Medium Priority recommendations.</p>");
        }

        sb.AppendLine("</div>");

        // Low Priority
        sb.AppendLine("<div class='card mb-4'>");
        sb.AppendLine("<h3 class='text-success'>Low Priority Recommendations</h3>");

        if (report.LowPriority.Any())
        {
            sb.AppendLine("<ul>");

            foreach (var item in report.LowPriority)
                sb.AppendLine($"<li>{item}</li>");

            sb.AppendLine("</ul>");
        }
        else
        {
            sb.AppendLine("<p>No Low Priority recommendations.</p>");
        }

        sb.AppendLine("</div>");

        // Conclusion
        sb.AppendLine("<div class='card'>");
        sb.AppendLine("<h3>Conclusion</h3>");
        sb.AppendLine($"<p>{report.Conclusion}</p>");
        sb.AppendLine("</div>");

        return template.Replace("{{RecommendationContent}}", sb.ToString());
    }
}