namespace McpClient.Contract
{
    public interface IMcpClientFactory
    {
        Task<ModelContextProtocol.Client.McpClient> CreateAsync(McpTransportType transportType);
    }
}
