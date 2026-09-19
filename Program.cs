using McpClient.Contract;
using McpClient.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModelContextProtocol.Client;
using OpenAI;

var builder = Host.CreateApplicationBuilder(args);

const string model = "gpt-5";

// Read API Key
string apiKey = builder.Configuration["OpenAI:ApiKey"]
                ?? Environment.GetEnvironmentVariable("OPENAI_API_KEY")
                ?? throw new InvalidOperationException("OpenAI API Key not found.");

builder.Services.AddSingleton(sp =>
{
    var openAiClient = new OpenAIClient(apiKey);

    return openAiClient.GetChatClient(model);
});

#region Agents



builder.Services.AddSingleton<DebtAgent>();

builder.Services.AddSingleton<RecommendationAgent>();

builder.Services.AddSingleton<HtmlRenderAgent>();
#endregion

#region Orchestrator

builder.Services.AddSingleton<SonarReportOrchestrator>();

#endregion

#region Services

builder.Services.AddSingleton<McpClientService>();
builder.Services.AddSingleton<HtmlRenderer>();
builder.Services.Configure<McpOptions>(
    builder.Configuration.GetSection("Mcp"));

builder.Services.AddScoped<IMcpClientFactory, McpClientFactory>();

#endregion

var host = builder.Build();

try
{
    var service =
        host.Services.GetRequiredService<McpClientService>();
    var input = "http";

    var transportType = input.ToLower() switch
    {
        "http" => McpTransportType.Http,
        "stdio" => McpTransportType.Stdio,
        _ => throw new ArgumentException("Invalid transport type.")
    };
    await service.RunAsync(transportType);
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(ex);
    Console.ResetColor();
}