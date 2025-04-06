using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationServiceMS.Models;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Threading.Channels;


namespace NotificationServiceMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly string _queueName = "notification_queue";
        private readonly string _rabbitMqUri = "amqps://bihjdlmj:y5EYXHG26Ktl6VfN7KU6c3aS3Z0XEUEz@possum.lmq.cloudamqp.com/bihjdlmj";

        [HttpPost]
        public IActionResult SendNotification([FromBody] NotificationMessage message)
        {
            try
            {
                var factory = new ConnectionFactory() { Uri = new Uri(_rabbitMqUri) };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false);

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(
                    exchange: "",
                    routingKey: _queueName,
                    basicProperties: properties,
                    body: body
                );

                return Ok("Notification published to RabbitMQ.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
