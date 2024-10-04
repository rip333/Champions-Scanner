Console.WriteLine("Enter Prompt:");
var prompt = Console.ReadLine();
var openAiService = new OpenAIService();
var response = await openAiService.AskChatGPT(prompt);
Console.WriteLine(response);

// Next is Vision
// https://github.com/openai/openai-dotnet/blob/main/examples/Chat/Example05_Vision.cs