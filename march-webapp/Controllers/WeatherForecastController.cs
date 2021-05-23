using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Azure.Messaging.ServiceBus;

namespace march_webapp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        static string connectionString = "Endpoint=sb://marchsbnamespace.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=BudeDVOB1pa7ROvOl181cj5cbgPnWIZaERUpISKHXX4=";
        static string topicName = "marchsbtopic";
        static string subscriptionName = "marchtopicsubscription";
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogInformation("Web api Start Get method invoke");
            var rng = new Random();
            _logger.LogInformation("Web api  End Get method invokee", rng);

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpPost]
        public async Task<string> PostAsync(string test)
        {
            _logger.LogInformation("Web api  Start Post method invoke");

            var rng = new Random();
            _logger.LogInformation("Web api  End Post method invoke", test);
            await SendMessageToTopicAsync(test);
            Task.WaitAll();
            return test;

        }
        static async Task SendMessageToTopicAsync(string test)
        {
            // create a Service Bus client 
            await using (ServiceBusClient client = new ServiceBusClient(connectionString))
            {
                // create a sender for the topic
                ServiceBusSender sender = client.CreateSender(topicName);
                await sender.SendMessageAsync(new ServiceBusMessage(test));
            }
        }
    }
}
