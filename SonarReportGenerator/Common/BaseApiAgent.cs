using OpenAI.Chat;
using System.Text.Json;

public abstract class BaseApiAgent
{
    protected readonly ChatClient ChatClient;

    private readonly string _prompt;

    protected BaseApiAgent(
        ChatClient chatClient,
        string promptFile)
    {
        ChatClient = chatClient;

        var promptPath = Path.Combine(
            AppContext.BaseDirectory,
               "SonarReportGenerator", "Agents",
            promptFile);

        if (!File.Exists(promptPath))
        {
            throw new FileNotFoundException(
                $"Prompt file not found: {promptPath}",
                promptPath);
        }

        _prompt = File.ReadAllText(promptPath);
    }

    // ----------------------------------------------------
    // Existing JSON-based execution
    // ----------------------------------------------------

    protected async Task<T> ExecuteAsync<TInput, T>(
        TInput input)
    {
        string json =
            JsonSerializer.Serialize(input);

        var messages =
            new List<ChatMessage>
            {
                new SystemChatMessage(_prompt),

                new UserChatMessage(json)
            };

        var completion =
            await ChatClient.CompleteChatAsync(
                messages);

        var response =
            string.Concat(
                completion.Value.Content
                    .Where(x =>
                        x.Kind ==
                        ChatMessageContentPartKind.Text)
                    .Select(x => x.Text));

        return JsonSerializer.Deserialize<T>(
            response,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
    }

    // ----------------------------------------------------
    // Existing text execution
    // ----------------------------------------------------

    protected async Task<string> ExecuteTextAsync<TInput>(
        TInput input)
    {
        string json =
            JsonSerializer.Serialize(input);

        var messages =
            new List<ChatMessage>
            {
                new SystemChatMessage(_prompt),

                new UserChatMessage(json)
            };

        var completion =
            await ChatClient.CompleteChatAsync(
                messages);

        return string.Concat(
            completion.Value.Content
                .Where(x =>
                    x.Kind ==
                    ChatMessageContentPartKind.Text)
                .Select(x => x.Text))
            .Trim();
    }

    // ----------------------------------------------------
    // NEW: Execute using raw text/context
    // ----------------------------------------------------

    protected async Task<string> ExecuteTextPromptAsync(
        string context)
    {
        var messages =
            new List<ChatMessage>
            {
                new SystemChatMessage(_prompt),

                new UserChatMessage(context)
            };

        var completion =
            await ChatClient.CompleteChatAsync(
                messages);

        return string.Concat(
            completion.Value.Content
                .Where(x =>
                    x.Kind ==
                    ChatMessageContentPartKind.Text)
                .Select(x => x.Text))
            .Trim();
    }
}