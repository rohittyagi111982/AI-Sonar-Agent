using MCPServerAPI.Model;
using MCPServerAPI.Models;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace MCPServerAPI;

[McpServerToolType]
public class SonarTools
{
    private readonly SonarService _service;

    public SonarTools(SonarService service)
    {
        _service = service;
    }

    [McpServerTool]
    [Description("Generates a complete SonarQube quality report for a project.")]
    public async Task<SonarReport> GetSonarReport(
        [Description("SonarQube project key")]
        string projectKey)
    {
        var issuesTask = _service.GetIssues(projectKey);
        var measuresTask = _service.GetMeasures(projectKey);
        var qualityGateTask = _service.GetQualityGates(projectKey);

        await Task.WhenAll(
            issuesTask,
            measuresTask,
            qualityGateTask);

        var issuesResult = await issuesTask;
        var measures = await measuresTask;
        var qualityGate = await qualityGateTask;

        var summary = BuildSummary(issuesResult.Issues);

        ApplyMeasures(summary, measures);

        summary.QualityGate =
            qualityGate?.ProjectStatus?.Status ?? "UNKNOWN";

        return new SonarReport
        {
            ProjectKey = projectKey,
            GeneratedOn = DateTime.UtcNow,
            Summary = summary,
            Files = BuildFileSummary(issuesResult.Issues),
            Issues = issuesResult.Issues
        };
    }

    private static SonarSummary BuildSummary(
        List<SonarIssue> issues)
    {
        return new SonarSummary
        {
            TotalIssues = issues.Count,

            Bugs = issues.Count(i =>
                i.Type.Equals(
                    "BUG",
                    StringComparison.OrdinalIgnoreCase)),

            Vulnerabilities = issues.Count(i =>
                i.Type.Equals(
                    "VULNERABILITY",
                    StringComparison.OrdinalIgnoreCase)),

            CodeSmells = issues.Count(i =>
                i.Type.Equals(
                    "CODE_SMELL",
                    StringComparison.OrdinalIgnoreCase)),

            Blocker = issues.Count(i =>
                i.Severity.Equals(
                    "BLOCKER",
                    StringComparison.OrdinalIgnoreCase)),

            Critical = issues.Count(i =>
                i.Severity.Equals(
                    "CRITICAL",
                    StringComparison.OrdinalIgnoreCase)),

            Major = issues.Count(i =>
                i.Severity.Equals(
                    "MAJOR",
                    StringComparison.OrdinalIgnoreCase)),

            Minor = issues.Count(i =>
                i.Severity.Equals(
                    "MINOR",
                    StringComparison.OrdinalIgnoreCase)),

            Info = issues.Count(i =>
                i.Severity.Equals(
                    "INFO",
                    StringComparison.OrdinalIgnoreCase))
        };
    }

    private static void ApplyMeasures(
        SonarSummary summary,
        MeasuresResponse? measures)
    {
        if (measures?.Component?.Measures == null)
            return;

        foreach (var measure in measures.Component.Measures)
        {
            switch (measure.Metric)
            {
                case "coverage":
                    summary.Coverage =
                        double.TryParse(
                            measure.Value,
                            out var coverage)
                            ? coverage
                            : 0;
                    break;

                case "duplicated_lines_density":
                    summary.Duplication =
                        double.TryParse(
                            measure.Value,
                            out var duplication)
                            ? duplication
                            : 0;
                    break;

                case "reliability_rating":
                    summary.Reliability =
                        ConvertRating(measure.Value);
                    break;

                case "security_rating":
                    summary.Security =
                        ConvertRating(measure.Value);
                    break;

                case "sqale_rating":
                    summary.Maintainability =
                        ConvertRating(measure.Value);
                    break;
            }
        }
    }

    private static string ConvertRating(string? value)
    {
        return value switch
        {
            "1.0" => "A",
            "2.0" => "B",
            "3.0" => "C",
            "4.0" => "D",
            "5.0" => "E",
            _ => "N/A"
        };
    }

    private static List<FileSummary> BuildFileSummary(
        List<SonarIssue> issues)
    {
        return issues
            .GroupBy(i => i.Component)
            .Select(g => new FileSummary
            {
                FileName = g.Key,
                TotalIssues = g.Count(),

                Bugs = g.Count(i =>
                    i.Type.Equals(
                        "BUG",
                        StringComparison.OrdinalIgnoreCase)),

                Vulnerabilities = g.Count(i =>
                    i.Type.Equals(
                        "VULNERABILITY",
                        StringComparison.OrdinalIgnoreCase)),

                CodeSmells = g.Count(i =>
                    i.Type.Equals(
                        "CODE_SMELL",
                        StringComparison.OrdinalIgnoreCase)),

                Blocker = g.Count(i =>
                    i.Severity.Equals(
                        "BLOCKER",
                        StringComparison.OrdinalIgnoreCase)),

                Critical = g.Count(i =>
                    i.Severity.Equals(
                        "CRITICAL",
                        StringComparison.OrdinalIgnoreCase)),

                Major = g.Count(i =>
                    i.Severity.Equals(
                        "MAJOR",
                        StringComparison.OrdinalIgnoreCase)),

                Minor = g.Count(i =>
                    i.Severity.Equals(
                        "MINOR",
                        StringComparison.OrdinalIgnoreCase))
            })
            .OrderByDescending(f => f.TotalIssues)
            .ToList();
    }
}