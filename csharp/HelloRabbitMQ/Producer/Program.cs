using System.Text;
using RabbitMQ.Client;

const string rabbitMQAddress = "localhost";

var factory = new ConnectionFactory() { HostName = rabbitMQAddress };
//factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
using var conn = factory.CreateConnection();
using var channel = conn.CreateModel();

channel.QueueDeclare("HelloRabbitQueue", false, false, false, null);

var message = "I'm just getting started with RabbitMQ";
var body = Encoding.UTF8.GetBytes(message);

channel.BasicPublish(string.Empty, "HelloRabbitQueue", null, body);

Console.WriteLine("Message sent to queue...");