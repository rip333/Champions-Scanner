using OpenAI;
using OpenAI.Chat;

public class OpenAIService
{
    private OpenAIClient _openAiClient;

    public OpenAIService()
    {
        var _apiKey = File.ReadAllText("token.txt").Trim();
        _openAiClient = new OpenAIClient(_apiKey);
    }

    public async Task<string> AskChatGPT(string prompt)
    {
        var client = _openAiClient.GetChatClient("gpt-4o");
        var completion = await client.CompleteChatAsync(new[] {
            new UserChatMessage(prompt)
        }
        );
        var resultString = completion.Value.Content[0].Text;
        Console.WriteLine($"[ASSISTANT]: {resultString}");
        return resultString;
    }
}
