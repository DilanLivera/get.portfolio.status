using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Post.Portfolio.Status.To.Slack
{
    public static class PostPortfolioStatusToSlack
    {
        private static HttpClient _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://hooks.slack.com")
        };

        private static string Uri = "/services/T018347L5QQ/B017KBF94TH/HvqdJ8AZFN7VaGOuQ0sUZTaj";

        [FunctionName("PostPortfolioStatusToSlack")]
        public static async Task<IActionResult> RunAsync(
            [TimerTrigger("* * 9 * * 6")] TimerInfo myTimer, 
            ILogger log)
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

            return new OkObjectResult(postResponse.StatusCode);
        }
    }
}
