using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;

namespace Mensageria.PoC.Api.Producer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AcademiaController : ControllerBase
    {
        private readonly ILogger<AcademiaController> _logger;

        public AcademiaController(ILogger<AcademiaController> logger)
        {
            _logger = logger;
        }

        [HttpGet("Aerobico")]
        public bool Aerobico()
        {
            ConnectionFactory factory = new()
            {
                VirtualHost = "academia",
                Uri = new Uri("amqp://guest:guest@host.docker.internal:5672/"),
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
                AutomaticRecoveryEnabled = true,
            };

            using (IConnection connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare("aerobico-exchange",  "topic");
                    channel.QueueDeclare(queue: $"aerobico-queue",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    channel.QueueBind(queue: $"aerobico-queue", exchange: "aerobico-exchange", routingKey: "aerobico-route");

                    string message = $"Ola DevInHouse Audaces {DateTime.Now}";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "aerobico-exchange",
                                         routingKey: "aerobico-route",
                                         basicProperties: null,
                                         body: body);

                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }

            return true;
        }

        [HttpGet("Musculacao")]
        public bool Musculacao()
        {
            ConnectionFactory factory = new()
            {
                VirtualHost = "academia",
                Uri = new Uri("amqp://guest:guest@host.docker.internal:5672/"),
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
                AutomaticRecoveryEnabled = true,
            };

            using (IConnection connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare("musculacao-exchange", "direct");
                    channel.QueueDeclare(queue: $"musculacao-queue",
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    channel.QueueBind(queue: $"musculacao-queue", exchange: "musculacao-exchange", routingKey: "musculacao-route");

                    string message = $"Ola DevInHouse Audaces {DateTime.Now}";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "musculacao-exchange",
                                         routingKey: "musculacao-route",
                                         basicProperties: null,
                                         body: body);

                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }

            return true;
        }
    }
}