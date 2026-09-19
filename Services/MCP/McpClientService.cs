using McpClient.Contract;
using McpClient.Services;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;
using System.Diagnostics;

public class McpClientService
{
    private readonly SonarReportOrchestrator _orchestrator;
    private readonly IMcpClientFactory _clientFactory;

    public McpClientService(
        SonarReportOrchestrator orchestrator,
        IMcpClientFactory clientFactory)
    {
        _orchestrator = orchestrator;
        _clientFactory = clientFactory;
    }

    public async Task RunAsync(McpTransportType transportType)
    {
        await using var client = await _clientFactory.CreateAsync(transportType);

        Console.WriteLine("Connected to MCP Server");

        var tools = await client.ListToolsAsync();

        Console.WriteLine("--------------------------------");
        Console.WriteLine("Available Tools");
        Console.WriteLine("--------------------------------");

        foreach (var tool in tools)
        {
            Console.WriteLine(tool.Name);
        }

        Console.WriteLine();
        Console.WriteLine("Getting SonarQube Report...");
        Console.WriteLine();

        var toolResult =
            await client.CallToolAsync(
                "get_sonar_report",
                new Dictionary<string, object?>
                {
                    ["projectKey"] = "Sonar-Project-V1"
                });

        string sonarJson =
            toolResult.Content
                .OfType<TextContentBlock>()
                .First()
                .Text;

        Console.WriteLine("Sonar report received.");

        Console.WriteLine("Generating AI HTML Report...");

        await GenerateReport(sonarJson);
    }

    private async Task GenerateReport(string sonarJson)
    {
        string html =
            await _orchestrator.GenerateReportAsync(
                sonarJson);

        string output =
            Path.Combine(
                AppContext.BaseDirectory,
                "SonarDashboard.html");
        html= await EmbedAssetsAsync(html);
        await File.WriteAllTextAsync(
            output,
            html);

        Console.WriteLine();

        Console.WriteLine("Opening report...");

        Process.Start(
            new ProcessStartInfo
            {
                FileName = output,
                UseShellExecute = true
            });

        Console.WriteLine();
        Console.WriteLine($"Report Generated Successfully");
        Console.WriteLine(output);
    }

    private async Task<string> EmbedAssetsAsync(string html)
    {
        string basePath = AppContext.BaseDirectory;

        string css1 = await File.ReadAllTextAsync(
            Path.Combine(basePath, "SonarReportGenerator", "Templates", "dashboard.css"));

        //string css2 = await File.ReadAllTextAsync(
        //    Path.Combine(basePath, "wwwroot", "css", "dashboard.css"));

        string js = await File.ReadAllTextAsync(
            Path.Combine(basePath, "SonarReportGenerator", "Templates", "dashboard.js"));

        html = html.Replace(
            "</head>",
            $"""
        <style>
        {css1}
        </style>       

        </head>
        """);

        html = html.Replace(
            "</body>",
            $"""
        <script>
        {js}
        </script>

        </body>
        """);

        return html;
    }
}