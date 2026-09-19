using McpClient.SonarReportGenerator.Models;
using OpenAI.Chat;

public class DebtAgent
    : BaseAgent<DebtReport>
{
    public DebtAgent(ChatClient client)
        : base(client, "DebtAgent.md")
    {
    }

    public Task<DebtReport> Execute(DebtReport request)
        => ExecuteAsync(request);
}