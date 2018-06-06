using System.Collections.Concurrent;


namespace RabbitWeApiDemo.MessageBusRepositories
{
    /// <summary>
    /// To 
    /// </summary>
    public class MessageRepositoryFactory<T> : IMessageRepositoryFactory<T>
    {
        private static ConcurrentDictionary<string, IMessageRepository<T>> messageRepositories = new ConcurrentDictionary<string, IMessageRepository<T>>();

        private IMessageRepository<T> CreateMessageRepository(string repositoryName)
        {
            IMessageRepository<T> messageRepository = new RabbitMessageRepository<T>();

            //Initialize message repository
            var messageQueueConfiguration = GetQueueConfiguration();
            messageQueueConfiguration.QueueName = repositoryName;
            messageRepository.Initialize(messageQueueConfiguration);

            //update dictory
            messageRepositories[repositoryName] = messageRepository;
            return messageRepository;
        }

        private MessageQueueConfiguration GetQueueConfiguration()
        {
            //TODO to take it from configuration
            var messageQueueConfiguration = new MessageQueueConfiguration()
            {
                HostName = "localhost",
            };
            return messageQueueConfiguration;
        }

        public MessageRepositoryFactory(string repositoryName)
        {
            RepositoryName = repositoryName;
            Create(repositoryName);
        }

        public string RepositoryName {get; set;}

        public IMessageRepository<T> Create(string repositoryName)
        {
            IMessageRepository<T> messageRepository;
            if ( messageRepositories.ContainsKey(repositoryName))
            {
                messageRepository = messageRepositories[repositoryName];
            }
            else
            {
                messageRepository = CreateMessageRepository(repositoryName);
            }

            return messageRepository;
        }

    }
}
