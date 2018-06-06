using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace RabbitWeApiDemo.MessageBusRepositories
{
    /// <summary>
    /// Generic rabbit message repository to publish/consume any object to/from queue
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RabbitMessageRepository<T> : IMessageRepository<T>
    {
        //TODO we can have some other way of managing messages instead of keeping it message queu
        private ConcurrentQueue<T> MessageQueue = new ConcurrentQueue<T>();

        private void ConsumerOnReceived(object sender, BasicDeliverEventArgs eventArguments)
        {
            var body = eventArguments.Body;
            T message = Deserialize(body);
            MessageQueue.Enqueue(message);
        }

        private static T Deserialize(byte[] body)
        {
            var messageAsString = Encoding.UTF8.GetString(body);
            T message = JsonConvert.DeserializeObject<T>(messageAsString);
            return message;
        }

        public IConnection Connection { get; set; }
        public IModel Channel { get; set; }

        public MessageQueueConfiguration MessageQueueConfiguration { get; set; }

        public void Initialize(MessageQueueConfiguration messageQueueConfiguration)
        {
            MessageQueueConfiguration = messageQueueConfiguration;
            var factory = new ConnectionFactory
            {
                HostName = messageQueueConfiguration.HostName,
            };

            Connection = factory.CreateConnection();
            Channel = Connection.CreateModel();
            
            Channel.QueueDeclare(queue: messageQueueConfiguration.QueueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

            var consumer = new EventingBasicConsumer(Channel);
            consumer.Received += ConsumerOnReceived;

            Channel.BasicConsume(queue: messageQueueConfiguration.QueueName,
                    autoAck:true,
                    consumer: consumer);
        }

        public void PostMessage(byte[] message)
        {
            Channel.BasicPublish(exchange: "",
                                     routingKey: MessageQueueConfiguration.QueueName,
                                     mandatory:true,
                                     basicProperties: null,
                                     body: message);
        }

        public T GetFirstMessage()
        {
            T messageObject;
            MessageQueue.TryDequeue(out messageObject);
            return messageObject;
        }

        public IEnumerable<T> GetAllMessages()
        {
            MessageQueue.TryDequeue(out T message);
            var messages = new List<T>
            {
                message
            };
            return messages;
        }

        public void Stop()
        {
            Channel.Close();
            Connection.Close();
        }

    }
}
