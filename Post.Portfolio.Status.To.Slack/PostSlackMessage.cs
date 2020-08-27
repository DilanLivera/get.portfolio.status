using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Post.Portfolio.Status.To.Slack
{
    public static class PostSlackMessage
    {
        private static readonly HttpClient _stockPriceClient = new HttpClient
        {
            BaseAddress = new Uri("https://www.alphavantage.co")
        };

        private static Dictionary<string, Stock> stocks = new Dictionary<string, Stock>
        {
            {
                "BRK.B" , new Stock { Code = "BRK.B", ExchnageCode = "NYSE" }
            },
            {
                "INDA" , new Stock { Code = "INDA", ExchnageCode = "BATS" }
            },
            {
                "MCHI" , new Stock { Code = "MCHI", ExchnageCode = "NASDAQ" }
            },
            {
                "SPCE" , new Stock { Code = "SPCE", ExchnageCode = "NYSE" }
            },
            {
                "TER" , new Stock { Code = "TER", ExchnageCode = "NASDAQ" }
            },
            {
                "VYM" , new Stock { Code = "VYM", ExchnageCode = "NYSEARCA" }
            },
        };

        [FunctionName("PostSlackMessage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            var apiKey = config["STOCKPRICEAPIKEY"];

            List<Task<Stock>> getStockPrice = stocks.Select(async stock =>
            {
                string uri = $"/query?function=TIME_SERIES_DAILY_ADJUSTED&symbol={stock.Key}&apikey={apiKey}";
                var request = new HttpRequestMessage(HttpMethod.Get, uri);

                using var response = await _stockPriceClient.SendAsync(request);

                if(response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                }

                return stock.Value;

            }).ToList();

            await Task.WhenAll(getStockPrice);

            log.LogInformation($"Http trigger function executed at: {DateTime.Now}");

            return new OkObjectResult("I'm Done");
        }
    }
}