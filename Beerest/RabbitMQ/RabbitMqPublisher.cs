using CheckService;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Beerest.RabbitMQ
{
    public class RabbitMqPublisher
    {
        private readonly string _rabbitMqQueueName = "check_queue";

        public async Task PublishMessageAsync(CreateCheckRequest request)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "user",
                Password = "rabbitmq"
            };

            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: _rabbitMqQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var message = JsonSerializer.Serialize(request, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            });

            Console.WriteLine($"Serialized message: {message}");

            var body = Encoding.UTF8.GetBytes(message);


            var properties = new BasicProperties
            {
                ContentType = "application/json",
                DeliveryMode = DeliveryModes.Persistent
            };

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: _rabbitMqQueueName,
                mandatory: false,
                basicProperties: properties,
                body: body);

            Console.WriteLine($"Message published to queue: {_rabbitMqQueueName}");
        }
    }
}
