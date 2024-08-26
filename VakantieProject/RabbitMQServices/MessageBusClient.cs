using VakantieProject.Dtos;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
namespace VakantieProject.RabbitMQServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration config;
        private readonly IConnection connection;
        private readonly IModel messageBusChannel;

        private readonly string queueName;
        public MessageBusClient(IConfiguration configuration)
        {
            config = configuration;
            var factory = new ConnectionFactory() { HostName = config["RabbitMQHost"], Port = int.Parse(config["RabbitMQPort"]) };
            try
            {
                connection = factory.CreateConnection();
                messageBusChannel = connection.CreateModel();

                messageBusChannel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Direct);

                connection.ConnectionShutdown += RabbitMQConnectionShutdown;

                queueName = messageBusChannel.QueueDeclare(
                    queue: "booking_service_qeue2",
                    durable: true,
                    exclusive: false,
                    autoDelete: false
                ).QueueName;

                messageBusChannel.QueueBind(queue: queueName, exchange: "trigger", routingKey: "created");

                Console.WriteLine("Connect to message bus");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> no messagebus: {ex.Message}");
            }
        }
        public void PublishNewCreatedHotel(HotelCreatedDto hotelCreatedDto)
        {
            var message = JsonSerializer.Serialize(hotelCreatedDto);

            if(connection.IsOpen)
            {
                Console.WriteLine("RabbitMQ connection open sending message now");
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("RabbitMQ connection not open not sending message");
            }
        }
        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            messageBusChannel.BasicPublish(exchange: "trigger", routingKey: "created", basicProperties: null, body: body);
            Console.WriteLine($" we have sent {message}");
        }

        public void Dispose()
        {
            Console.WriteLine("Messagebus disposed");
            if (messageBusChannel.IsOpen)
            {
                messageBusChannel.Close();
            }
        }

        private void RabbitMQConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("MessageBus Rabbit mq is shut down");
        }
    }
}
