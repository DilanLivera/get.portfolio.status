using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
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
        private static HttpClient _slackClient = new HttpClient
        {
            BaseAddress = new Uri("https://hooks.slack.com")
        };

        [FunctionName("PostSlackMessage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            var slackUri = config["SLACKURI"];

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

            var request = new HttpRequestMessage(HttpMethod.Post, slackUri)
            {
                Content = JsonContent.Create(slackMessage)
            };

            var postResponse = await _slackClient.SendAsync(request);

            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            log.LogInformation($"Slack returned status code: {postResponse.StatusCode}");

            return new OkObjectResult(postResponse.StatusCode);
        }
    }
}
