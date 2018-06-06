using System.Collections.Generic;

namespace RabbitWeApiDemo.MessageBusRepositories
{
    public interface IMessageRepository<T>
    {
        /// <summary>
        /// To initialize message queue setup
        /// </summary>
        /// <param name="messageQueueConfiguration"></param>
        void Initialize(MessageQueueConfiguration messageQueueConfiguration);

        /// <summary>
        /// To post message to queue
        /// </summary>
        /// <param name="message"></param>
        void PostMessage(byte[] message);

        /// <summary>
        /// To get first added message to queue
        /// </summary>
        /// <returns></returns>
        T GetFirstMessage();

        /// <summary>
        /// To get all the messages from queue
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAllMessages();
    }
}
