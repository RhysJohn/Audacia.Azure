using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Audacia.Azure.StorageQueue.Models;

namespace Audacia.Azure.StorageQueue.Services.Interfaces
{
    /// <summary>
    /// Get service for returning messages from an Azure Queue Storage account.
    /// </summary>
    public interface IGetAzureQueueStorageService
    {
        /// <summary>
        /// Returns the top message from a queue.
        /// </summary>
        /// <param name="queueName">Name of the queue which you want to take the next message from.</param>
        /// <param name="deleteMessageAfterReceiving">
        /// Whether you want to remove the messages from the queue after they have been received.
        /// </param>
        /// <returns>The message from the queue if there is one.</returns>
        Task<AzureQueueStorageMessage> GetAsync(string queueName, bool deleteMessageAfterReceiving = true);

        /// <summary>
        /// Return a custom amount of messages from a queue.
        /// </summary>
        /// <param name="queueName">Name of queue which you want to get the messages from.</param>
        /// <param name="amountToReceive">The amount of messages you want to get from the queue.</param>
        /// <param name="deleteMessageAfterReceiving">
        /// Whether you want to remove the messages from the queue after they have been received.
        /// </param>
        /// <returns>
        /// A collection of size <paramref name="amountToReceive"/> of <see cref="AzureQueueStorageMessage"/>'s if
        /// available if not the remaining amount that are currently in the queue.
        /// </returns>
        Task<IEnumerable<AzureQueueStorageMessage>> GetSomeAsync(string queueName,
            int amountToReceive,
            bool deleteMessageAfterReceiving = true);
    }
}