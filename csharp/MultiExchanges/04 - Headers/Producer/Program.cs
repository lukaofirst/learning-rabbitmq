using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory();
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare("HeadersExchange", ExchangeType.Headers);

Console.WriteLine("How many message do you want to create?");
var times = int.Parse(Console.ReadLine()!);
var rnd = new Random();

for (int i = 0; i < times; i++)
{
	var message = Encoding.UTF8.GetBytes($"Message #{i}");
	var properties = channel.CreateBasicProperties();
	properties.Headers = new Dictionary<string, object>() 
	{
		{ "format", "pdf" },
		{ "type", "report" }
	};

	channel.BasicPublish("HeadersExchange", string.Empty, properties, message);
}