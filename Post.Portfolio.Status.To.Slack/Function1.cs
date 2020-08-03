using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Post.Portfolio.Status.To.Slack
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run([TimerTrigger("* * 9 * * 6")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
