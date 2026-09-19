using MCPServerAPI;
using ModelContextProtocol.Server;

var builder = WebApplication.CreateBuilder(args);

// Register SonarQube service
builder.Services.AddSingleton<SonarService>();


// MCP Server
builder.Services
    .AddMcpServer()
    .WithHttpTransport(options =>
    {
        options.Stateless = true;
    })
    .WithTools<SonarTools>();

var app = builder.Build();

//app.UseHttpsRedirection();

app.MapMcp("/mcp");

app.Run();