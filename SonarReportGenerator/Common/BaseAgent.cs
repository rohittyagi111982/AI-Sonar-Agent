using OpenAI.Chat;
using System.Text.Json;

public abstract class BaseAgent<T>
{
    protected readonly ChatClient ChatClient;
    private readonly string _prompt;

    protected BaseAgent(ChatClient chatClient, string promptFile)
    {
        ChatClient = chatClient;

        var promptPath = Path.Combine(
            AppContext.BaseDirectory,
            "SonarReportGenerator", "Instructions",
            promptFile);

        _prompt = File.ReadAllText(promptPath);
    }

    // For agents that return JSON
    protected async Task<T> ExecuteAsync<TInput>(TInput input)
    {
        string json = JsonSerializer.Serialize(input);

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(_prompt),
            new UserChatMessage(json)
        };

        var completion = await ChatClient.CompleteChatAsync(messages);
        var response = string.Concat(
            completion.Value.Content
                .Where(x => x.Kind == ChatMessageContentPartKind.Text)
                .Select(x => x.Text))
            .Trim();

        Console.WriteLine("========== AI RESPONSE ==========");
        Console.WriteLine(response);
        Console.WriteLine("================================");

        return JsonSerializer.Deserialize<T>(
            response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    }

    // For agents that return plain text
    protected async Task<string> ExecuteTextAsync<TInput>(TInput input)
    {
        string json = JsonSerializer.Serialize(input);

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(_prompt),
            new UserChatMessage(json)
        };

        var completion = await ChatClient.CompleteChatAsync(messages);

        return string.Concat(
            completion.Value.Content
                .Where(x => x.Kind == ChatMessageContentPartKind.Text)
                .Select(x => x.Text))
            .Trim();


    }
}