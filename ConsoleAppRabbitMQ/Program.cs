
using Newtonsoft.Json;
using RabbitMQ.Client;
using Shared;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri(""); //write AMQP URL

string messageRead;

do
{
    Console.Write("Please write your message for 50 times: ");
    messageRead = Console.ReadLine();
    if (messageRead != null)
    {
        PublishMessage(messageRead);
    }
    else
    {
        Environment.Exit(0);
    }

} while (messageRead != null);

void PublishMessage(string message)
{
    Random randomEnumLogName = new Random();

    try
    {
        using (var connection = factory.CreateConnection())
        {
            var channel = connection.CreateModel();

            channel.ExchangeDeclare("exchange-header-logs", durable: true, type: ExchangeType.Headers);

            Dictionary<string, object> header = new Dictionary<string, object>();

            header.Add("format", "pdf");
            header.Add("shape", "a4");

            var property = channel.CreateBasicProperties();

            property.Headers = header;
            property.Persistent = true;

            Customer customer = new Customer() { Id = 1, FirstName = "Mehmet", LastName = "Kar", Phone = "565898587", Address = "USA" };

            var JsonCustomer = JsonConvert.SerializeObject(customer);

            channel.BasicPublish("exchange-header-logs", string.Empty, property, Encoding.UTF8.GetBytes(JsonCustomer));

            Console.WriteLine("All Logs are sended");
        }
    }
    catch (Exception ex) { Console.WriteLine(ex.ToString()); }
}


