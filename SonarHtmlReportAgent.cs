using Microsoft.Extensions.AI;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;
using OpenAI.Chat;

public class SonarHtmlReportAgent
{
    private readonly ChatClient _chatClient;

    public SonarHtmlReportAgent(ChatClient chatClient)
    {
        _chatClient = chatClient;
    }

    public async Task<string> GenerateHtmlReportAsync(
        ModelContextProtocol.Client.McpClient client,
        string projectKey)
    {
        Console.WriteLine("Calling MCP Tool...");

        var result =
            await client.CallToolAsync(
                "get_sonar_report",
                new Dictionary<string, object?>
                {
                    ["projectKey"] = projectKey
                });

        string json =
            result.Content
                .OfType<TextContentBlock>()
                .First()
                .Text;

        Console.WriteLine("Generating HTML using GPT...");

        string prompt = BuildPrompt(json);

        var completion =
            await _chatClient.CompleteChatAsync(prompt);

        foreach (var item in completion.Value.Content)
        {
            Console.WriteLine(item.GetType().FullName);
        }

       
        return string.Concat(
    completion.Value.Content
        .Where(p => p.Kind == ChatMessageContentPartKind.Text)
        .Select(p => p.Text));
      
    }

    private static string BuildPrompt(string json)
    {
        return $"""
You are a Principal Software Architect and Senior UI/UX Designer.

Your responsibility is to generate a professional, enterprise-grade SonarQube HTML Dashboard suitable for CTOs, architects, managers, and developers.

Return ONLY valid HTML.
Do not return Markdown.
Do not explain anything.
Do not wrap the HTML inside code fences.

Use Bootstrap 5 CDN.

Use Bootstrap Icons CDN.

Use Chart.js CDN.

Use DataTables CDN.

Everything must be contained in a single HTML file.

The report must be completely responsive.

Use a modern enterprise dashboard style similar to Azure DevOps, GitHub Enterprise, SonarQube Enterprise or Power BI.

Use shadows, rounded cards, spacing and professional colors.

Never use inline styles unless absolutely required.

Generate readable semantic HTML.

Use Bootstrap Cards.

Use Bootstrap Tables.

Use Accordions where appropriate.

Use Progress Bars.

Use Badges.

Use Alerts.

Use Sticky Navigation.

Use Tabs.

Use Tooltips.

Use Collapse controls.

Use responsive DataTables.

Generate charts using Chart.js.

Do not omit any section even if there is no data.

If JSON does not contain a value display "N/A".

Never invent metrics.

Input JSON

{json}
""";
    }
}