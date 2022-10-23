using Mensageria.PoC.Api.Producer;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;

namespace Mensageria.PoC.Api.Producer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SendController : ControllerBase
    {
        private readonly ILogger<SendController> _logger;

        public SendController(ILogger<SendController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "Get")]
        public bool Get()
        {
            ConnectionFactory factory = new()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                ContinuationTimeout = TimeSpan.MaxValue
            };

            using (IConnection connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: $"Ola DevInHouse Audaces {DateTime.Now}",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    string message = "Hello World!";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                         routingKey: "hello",
                                         basicProperties: null,
                                         body: body);
                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }

            return true;
        }
    }
}