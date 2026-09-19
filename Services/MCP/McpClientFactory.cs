using McpClient.Contract;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Client;

public class McpClientFactory : IMcpClientFactory
{
    private readonly McpOptions _options;

    public McpClientFactory(IOptions<McpOptions> options)
    {
        _options = options.Value;
    }

    public async Task<ModelContextProtocol.Client.McpClient> CreateAsync(
        McpTransportType transportType)
    {
        IClientTransport transport;

        switch (transportType)
        {
            case McpTransportType.Http:

                // Pick Endpoint from appsettings.json
                var endpoint = _options.Http.Endpoint;

                transport = new HttpClientTransport(new()
                {
                    Endpoint = new Uri(endpoint),
                    Name = _options.Http.Name
                });

                break;

            case McpTransportType.Stdio:

                transport = new StdioClientTransport(new()
                {
                    Command = _options.Stdio.Command,
                    Name = _options.Stdio.Name,
                    Arguments =
                    [
                        _options.Stdio.ServerPath
                    ]
                });

                break;

            default:
                throw new ArgumentOutOfRangeException(
                    nameof(transportType),
                    transportType,
                    "Unsupported MCP transport.");
        }

        return await ModelContextProtocol.Client.McpClient.CreateAsync(transport);
    }
}