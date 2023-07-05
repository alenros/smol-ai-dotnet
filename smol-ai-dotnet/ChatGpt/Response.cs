using System.Text.Json.Serialization;

namespace ChatGPT
{

    public class Response
    {
        [JsonPropertyName("error")]
        public Error Error { get; set; }
        
        [JsonPropertyName("choices")]
        public List<Choice> Choices { get; set; }
    }

    public class Error
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }

    public class Choice
    {
        [JsonPropertyName("message")]
        public Message Message { get; set; }

        public string text { get; set; }

        public int index { get; set; }

        public object logprobs { get; set; }

        [JsonPropertyName("finish_reason")]
        public string FinishReason { get; set; }
    }
}
