using ChatGPT;
using System.Data.SqlClient;
using System.Text;

public class Program
{

    const string apiKey = "===API_KEY===";
    const string modelName = "gpt-3.5-turbo"; // or 'gpt-4'
    const int maxTokensForModel = 2048;

    public static async Task<string> GenerateResponse(string systemPrompt, string userPrompt)
    {
        ReportTokens(systemPrompt);
        ReportTokens(userPrompt);

        var messages = new List<Message>
        {
            new Message { Role = "system", Content = systemPrompt },
            new Message { Role = "user", Content = userPrompt }
        };

        var parameters = new ChatCompletionParams
        {
            Model = modelName,
            Messages = messages,
            MaxTokens = maxTokensForModel,
            Temperature = 0
        };

        var client = new OpenAiClient(apiKey);
        var response = await client.CallChatCompletionAsync(parameters);

        if (response.Error is Error error)
        {
            // response.Error "That model is currently overloaded with other requests. You can retry your request, or contact us through our help center at help.openai.com if the error persists. (Please include the request ID b24afc2c91294744679576584d2954b2 in your message.)"
            return error.Message;
        }

        var reply = response.Choices.FirstOrDefault().Message.Content;
        return reply;
    }

    private static void ReportTokens(string prompt)
    {
        // TODO make this log instead of console
        return;
        Console.WriteLine($"{prompt.Length} tokens in the prompt: {prompt}");
    }

    public static async Task MainAsync(string prompt)
    {
        var setColorToGreen = "\u001b[92m";
        var setColorBack = "\u001b[0m";
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("Your Question:");
        Console.WriteLine($"{setColorToGreen}{prompt}{setColorBack}");

        var sqlQuery = await GenerateResponse(
       // Assistant content
       // Assistant
       $@"Given an input question, respond with syntactically correct T-SQL.
Be creative but the SQL must be correct.
Only use tables called ""table1"" and ""table2"".
The ""table1"" table has columns: column1 (string), column2 (Integer).
The ""table2"" table has columns: column3 (string), column4(integer)."
           ,
        // User
        $"Question: {prompt}"
);

        Console.WriteLine(sqlQuery);
        await Console.Out.WriteLineAsync();
        var result = QueryDatabase(sqlQuery);
        await Console.Out.WriteLineAsync(result);
    }

    private static string QueryDatabase(string sqlQuery)
    {
        var connectionString = "MyConnetionString";
        var connection = new SqlConnection(connectionString);
        connection.Open();
        //connection.BeginTransaction();

        var command = new SqlCommand(sqlQuery, connection);
        var reader = command.ExecuteReader();
        // Execute the query and obtain a result set
        var resultBuilder = new StringBuilder();
        while (reader.Read())
        {
            var t1 = reader[0].ToString();
            resultBuilder.AppendLine(t1);
        }


        return resultBuilder.ToString();
    }

    static async Task Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Please provide your question as the first argument.");
            return;
        }

        var prompt = args[0];

        await MainAsync(prompt);
    }
}
