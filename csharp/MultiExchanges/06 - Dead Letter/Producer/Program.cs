using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory();
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare("DeadLetterExchange", ExchangeType.Direct);

channel.QueueDeclare("dead-letter-queue", false, false, false, null);
channel.QueueBind("dead-letter-queue", "DeadLetterExchange", "dead-letter-routing-key");

var arguments = new Dictionary<string, object>()
{
	{ "x-dead-letter-exchange", "DeadLetterExchange" },
	{ "x-dead-letter-routing-key", "dead-letter-routing-key" }
};

channel.QueueDeclare("main-queue", false, false, false, arguments);

Console.WriteLine("How many message do you want to create?");
var times = int.Parse(Console.ReadLine()!);
var rnd = new Random();

for (int i = 0; i < times; i++)
{
	var message = Encoding.UTF8.GetBytes($"Message #{i}");
	var properties = channel.CreateBasicProperties();
	properties.Expiration = "5000";

	/* Use an empty string as the first argument, the default exchange type is been used here (Direct) */

	/* 
	   The second argument specifies the routing key for the message, 
	   but in this case, the routing key is the name of the queue,
	   because of the default exchange type
	*/
	channel.BasicPublish(string.Empty, "main-queue", properties, message);
}