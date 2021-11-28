using System.Threading.Tasks;

namespace Audacia.Azure.StorageQueue.Services.Interfaces
{
    /// <summary>
    /// Delete service for removing messages from an Azure Queue Storage account.
    /// </summary>
    public interface IDeleteAzureQueueStorageService
    {
        /// <summary>
        /// Removing a message from a queue.
        /// </summary>
        /// <param name="queueName">The name of the queue you want to remove a message from.</param>
        /// <param name="messageId">Id of the message which you want to delete.</param>
        /// <returns>Bool whether the message was deleted from the queue.</returns>
        Task<bool> ExecuteAsync(string queueName, string messageId);
    }
}