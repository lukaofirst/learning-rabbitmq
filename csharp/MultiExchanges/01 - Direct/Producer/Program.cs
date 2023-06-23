using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672 };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare("DirectExchange", ExchangeType.Direct);
channel.QueueDeclare("example-queue", false, false, false, null);
channel.QueueBind("example-queue", "DirectExchange", "routingKey");

Console.WriteLine("How many message do you want to create?");
var times = int.Parse(Console.ReadLine()!);


for (int i = 0; i < times; i++)
{
	var message = Encoding.UTF8.GetBytes($"Message #{i}");

	channel.BasicPublish("DirectExchange", "routingKey", null, message);
}