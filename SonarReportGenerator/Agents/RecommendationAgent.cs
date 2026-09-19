using McpClient.SonarReportGenerator.Models;
using OpenAI.Chat;

public class RecommendationAgent
    : BaseAgent<RecommendationReport>
{
    public RecommendationAgent(ChatClient client)
        : base(client, "RecommendationAgent.md")
    {
    }

    public Task<RecommendationReport> Execute(RecommendationInput json)
        => ExecuteAsync(json);
}