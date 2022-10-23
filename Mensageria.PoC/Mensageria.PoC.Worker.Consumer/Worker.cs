using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Mensageria.PoC.Worker.Consumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ConnectionFactory factory = new()
            {
                VirtualHost = "academia",
                Uri = new Uri("amqp://guest:guest@host.docker.internal:5672/"),
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10),
                AutomaticRecoveryEnabled = true,
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: $"aerobico-queue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);


            channel.QueueDeclare(queue: $"musculacao-queue",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            
            var consumerAerobico = new EventingBasicConsumer(channel);

            consumerAerobico.Received += (model, ea) =>
            {
                ea.RoutingKey = "aerobico-route";
                ea.Exchange = "aerobico-exchange";
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);
            };

            var consumerMusculacao = new EventingBasicConsumer(channel);


            consumerMusculacao.Received += (model, ea) =>
            {
                ea.RoutingKey = "musculacao-route";
                ea.Exchange = "musculacao-exchange";
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);
            };

            channel.BasicConsume(queue: "aerobico-queue",
                                 autoAck: true,
                                 consumer: consumerAerobico);

            channel.BasicConsume(queue: "musculacao-queue",
                                 autoAck: true,
                                 consumer: consumerMusculacao);

            while (!stoppingToken.IsCancellationRequested)
            {


                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}