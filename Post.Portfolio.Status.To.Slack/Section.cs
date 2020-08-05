using System.Text.Json.Serialization;

namespace Post.Portfolio.Status.To.Slack
{
    public class Section
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("text")]
        public Text Text { get; set; }
    }
}
