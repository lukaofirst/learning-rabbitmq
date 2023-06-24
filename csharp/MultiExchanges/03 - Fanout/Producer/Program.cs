using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory();
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare("FanoutExchange", ExchangeType.Fanout);

Console.WriteLine("How many message do you want to create?");
var times = int.Parse(Console.ReadLine()!);
var rnd = new Random();

for (int i = 0; i < times; i++)
{
	var message = Encoding.UTF8.GetBytes($"Message #{i}");

	channel.BasicPublish("FanoutExchange", string.Empty, null, message);
}