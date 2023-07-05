
using System.Text.Json.Serialization;

namespace ChatGPT
{
    public class ChatCompletionParams
    {
        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("messages")]
        public List<Message> Messages { get; set; }

        [JsonPropertyName("max_tokens")]
        public int MaxTokens { get; set; }

        [JsonPropertyName("temperature")]
        public int Temperature { get; set; }
    }
}