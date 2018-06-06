namespace RabbitWeApiDemo.MessageBusRepositories
{
    public interface IMessageRepositoryFactory<T>
    {
        /// <summary>
        /// To create message repository
        /// </summary>
        /// <param name="repositoryName"></param>
        /// <returns></returns>
        IMessageRepository<T> Create(string repositoryName);

        string RepositoryName { get; set; }
    }
}
