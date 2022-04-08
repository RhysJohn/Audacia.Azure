using System.Linq;
using System.Threading.Tasks;
using Audacia.Azure.StorageQueue.Config;
using Audacia.Azure.StorageQueue.Extensions;
using Audacia.Azure.StorageQueue.Services.Interfaces;
using Azure;
using Azure.Storage.Queues;
using Microsoft.Extensions.Options;

namespace Audacia.Azure.StorageQueue.Services
{
    /// <summary>
    /// Delete service for removing messages from an Azure Queue Storage account.
    /// </summary>
    public class DeleteAzureQueueStorageService : BaseQueueStorageService, IDeleteAzureQueueStorageService
    {
        /// <summary>
        /// Constructor option for when adding the <see cref="QueueClient"/> has being added to the DI.
        /// </summary>
        /// <param name="queueClient"></param>
        protected DeleteAzureQueueStorageService(QueueClient queueClient) : base(queueClient)
        {
        }

        /// <summary>
        /// Constructor option for using the Options pattern with <see cref="QueueStorageOption"/>. 
        /// </summary>
        /// <param name="queueStorageConfig"></param>
        protected DeleteAzureQueueStorageService(IOptions<QueueStorageOption> queueStorageConfig) : base(
            queueStorageConfig)
        {
        }

        /// <summary>
        /// Removing a message from a queue.
        /// </summary>
        /// <param name="queueName">The name of the queue you want to remove a message from.</param>
        /// <param name="messageId">Id of the message which you want to delete.</param>
        /// <returns>Bool whether the message was deleted from the queue.</returns>
        public async Task<bool> ExecuteAsync(string queueName, string messageId)
        {
            await PreQueueChecksAsync(queueName);

            var queueMessages = await QueueClient.ReceiveMessagesAsync();

            var peekedMessages = queueMessages.Value;

            var peekMessage = peekedMessages.WithMessageId(messageId).FirstOrDefault();

            if (peekMessage != null)
            {
                var deleteResponse = await QueueClient.DeleteMessageAsync(messageId, peekMessage.PopReceipt);

                return deleteResponse.Status == 200;
            }

            return false;
        }
    }
}