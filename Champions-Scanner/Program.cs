// if (args.Length == 0){
//     Console.WriteLine("No arguments provided.");
// }
// var filePath = args[0];

var openAiService = new OpenAIService();
await openAiService.GetJsonFromAssets();
