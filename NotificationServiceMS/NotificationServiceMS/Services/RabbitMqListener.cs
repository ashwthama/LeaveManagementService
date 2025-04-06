using Microsoft.EntityFrameworkCore.Metadata;
using NotificationServiceMS.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using IModel = RabbitMQ.Client.IModel;

namespace NotificationServiceMS.Services
{
    public class RabbitMqListener :BackgroundService
    {
        private readonly string _queueName = "notification_queue";
        private readonly string _rabbitMqUri = "amqps://bihjdlmj:y5EYXHG26Ktl6VfN7KU6c3aS3Z0XEUEz@possum.lmq.cloudamqp.com/bihjdlmj";
        private IConnection _connection;
        private IModel _channel;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory() { Uri = new Uri(_rabbitMqUri) };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, e) =>
            {
                var body = Encoding.UTF8.GetString(e.Body.ToArray());
                var message = JsonSerializer.Deserialize<NotificationMessage>(body);

                var handler = new NotificationHandler();
                handler.Send(message);

                _channel.BasicAck(e.DeliveryTag, false);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }
}
