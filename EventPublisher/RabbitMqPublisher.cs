using System.Text.Json;
using Core.Settings;
using EventPublisher.Contracts;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Database.Entities;

//
// EXAMPLE OF USING MESSAGE BROKER PUBLISHER
//
namespace EventPublisher
{
    public class RabbitMqPublisher
    {
        private readonly string _rabbitMqConnectionString;

        private readonly string _environmentPrefix;

        public RabbitMqPublisher(IOptions<EventBusSettings> settings)
        {
            _rabbitMqConnectionString = settings.Value.HostAddress;
            _environmentPrefix = settings.Value.EnvironmentPrefix;
        }
        
        public void PublishInputFileCreatedEvent(InputHtmlFileEntity entity)
        {
            var contract = FileContract.ConvertToEventContract(entity);
            SendMessage("html-service:input-file-created", contract);
        }

        private void SendMessage(string exchangeName, object contract)
        {
            using (IConnection connection = GetConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(GetExchangeName(exchangeName), type: ExchangeType.Fanout, durable: true, autoDelete: false, arguments: null);
                    channel.BasicPublish(GetExchangeName(exchangeName), routingKey: "", mandatory: false, basicProperties: null, body: JsonSerializer.SerializeToUtf8Bytes(contract));
                }
            }
        }

        private IConnection GetConnection()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(_rabbitMqConnectionString)
            };

            return connectionFactory.CreateConnection();
        }

        private string GetExchangeName(string name)
        {
            return $"{_environmentPrefix}:{name}";
        }
    }
}