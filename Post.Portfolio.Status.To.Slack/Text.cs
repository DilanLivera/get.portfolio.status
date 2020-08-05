using System.Text.Json.Serialization;

namespace Post.Portfolio.Status.To.Slack
{
    public class Text
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("text")]
        public string Message { get; set; }
    }
}
