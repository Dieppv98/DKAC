using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace DKAC.Common
{
    public class RabbitMQB
    {
        public IConnection GetConnection()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.UserName = "guest";
            factory.Password = "guest";
            factory.Port = 5672;
            factory.HostName = "localhost";
            factory.VirtualHost = "/";
            // factory.Uri = "http://192.168.7.140:15672/";
            return factory.CreateConnection();
        }

        public bool send(IConnection con, string message, string end)
        {
            try
            {
                IModel channel = con.CreateModel();
                channel.ExchangeDeclare("messageexchange", ExchangeType.Direct);
                channel.QueueDeclare(end, true, false, false, null);
                channel.QueueBind(end, "messageexchange", end, null);
                var msg = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("messageexchange", end, null, msg);

            }
            catch (Exception ex)
            {
                var err = ex.Message;
            }
            return true;

        }

        public string receive(IConnection con, string myqueue)
        {
            try
            {
                string queue = myqueue;
                IModel channel = con.CreateModel();
                channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
                var consumer = new EventingBasicConsumer(channel);
                BasicGetResult result = channel.BasicGet(queue: queue, autoAck: true);
                if (result != null)
                    return Encoding.UTF8.GetString(result.Body.ToArray());
                else
                    return null;
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                return null;

            }

        }
    }
}