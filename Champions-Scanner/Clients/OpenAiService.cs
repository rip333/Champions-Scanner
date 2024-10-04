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

    public async Task GetJsonFromAssets()
    {
        // Get the path to the assets folder
        var assetsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "assets");

        // Get all the image files in the assets folder
        var imageFiles = Directory.GetFiles(assetsFolderPath, "*.png");

        // Iterate through each image file and call GetJsonFromCardImage
        foreach (var imageFile in imageFiles)
        {
            var result = await GetJsonFromCardImage(imageFile);
            // Save each result as a JSON file
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(imageFile);
            var outputFilePath = Path.Combine(assetsFolderPath, $"{fileNameWithoutExtension}_response.json");

            var trimmedResult = TrimOutsideBraces(result);
            // Write JSON response to a file
            await File.WriteAllTextAsync(outputFilePath, trimmedResult);
        }
    }

    public async Task<string> GetJsonFromCardImage(string file)
    {
        var client = _openAiClient.GetChatClient("gpt-4o");
        using Stream imageStream = File.OpenRead(file);
        BinaryData imageBytes = BinaryData.FromStream(imageStream);
        if (imageBytes.ToArray().Length > 0)
        {
            Console.WriteLine($"{file} image loaded.");
        }
        else
        {
            Console.WriteLine("Failed to load image.");
            throw new Exception("Failed to load image.");
        }
        string promptFilePath = Path.Combine("prompts", "prompt1.txt");
        var prompt = File.ReadAllText(promptFilePath).Trim();
        var messageContent = new List<ChatMessageContentPart> {
                ChatMessageContentPart.CreateTextMessageContentPart(prompt),
                ChatMessageContentPart.CreateImageMessageContentPart(imageBytes, "image/png") };

        var messages = new List<ChatMessage>
        {
            new UserChatMessage(messageContent)
        };
        ChatCompletion completion = await client.CompleteChatAsync(messages);
        Console.WriteLine($"{file}: Complete.");
        return completion.Content[0].Text;
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

    public string TrimOutsideBraces(string response)
    {
        // Find the first occurrence of '{' and the last occurrence of '}'
        int startIndex = response.IndexOf('{');
        int endIndex = response.LastIndexOf('}');

        // If both braces are found, return the substring between them
        if (startIndex >= 0 && endIndex >= 0 && endIndex > startIndex)
        {
            return response.Substring(startIndex, (endIndex - startIndex) + 1);
        }

        // Return the original string if something goes wrong
        return response;
    }
}
