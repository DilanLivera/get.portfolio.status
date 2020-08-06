using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Post.Portfolio.Status.To.Slack
{
    public static class PostSlackMessage
    {
        private static HttpClient _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://hooks.slack.com")
        };

        private static string Uri = "/services/T018347L5QQ/B017KBF94TH/HvqdJ8AZFN7VaGOuQ0sUZTaj";

        [FunctionName("PostSlackMessage")]
        public static async Task RunAsync([TimerTrigger("*/5 * * * *")] TimerInfo myTimer, ILogger log)
        {
            var slackMessage = new Dictionary<string, List<Section>>
            {
                {
                    "blocks", new List<Section>
                    {
                        new Section
                        {
                            Type = "section",
                            Text = new Text
                            {
                                Type = "mrkdwn",
                                Message = "*Hello* and _world_."
                            }
                        }
                    }
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, Uri)
            {
                Content = JsonContent.Create(slackMessage)
            };

            var postResponse = await _httpClient.SendAsync(request);

            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            log.LogInformation($"Slack returned status code: {postResponse.StatusCode}");
        }
    }
}
