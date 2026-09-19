public class McpOptions
{
    public HttpMcpOptions Http { get; set; } = new();
    public StdioMcpOptions Stdio { get; set; } = new();
}

public class HttpMcpOptions
{
    public string Endpoint { get; set; } = string.Empty;
    public string Name { get; set; } = "Sonar Client";
}

public class StdioMcpOptions
{
    public string Command { get; set; } = "dotnet";
    public string ServerPath { get; set; } = string.Empty;
    public string Name { get; set; } = "Sonar Client";
}