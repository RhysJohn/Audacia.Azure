using System.Threading.Tasks;
using Audacia.Azure.StorageQueue.Config;
using Audacia.Azure.StorageQueue.Services.Interfaces;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Options;

namespace Audacia.Azure.StorageQueue.Services
{
    /// <summary>
    /// Add service for adding messages to an Azure Queue Storage account.
    /// </summary>
    public class AddAzureQueueStorageService : BaseQueueStorageService, IAddAzureQueueStorageService
    {
        /// <summary>
        /// Constructor option for when adding the <see cref="QueueClient"/> has being added to the DI.
        /// </summary>
        /// <param name="queueClient"></param>
        public AddAzureQueueStorageService(QueueClient queueClient) : base(queueClient)
        {
        }

        /// <summary>
        /// Constructor option for using the Options pattern with <see cref="QueueStorageOption"/>. 
        /// </summary>
        /// <param name="queueStorageConfig"></param>
        public AddAzureQueueStorageService(IOptions<QueueStorageOption> queueStorageConfig) : base(queueStorageConfig)
        {
        }

        /// <summary>
        /// Add a new message to a queue.
        /// </summary>
        /// <param name="queueName">Name of the queue which the message will be added too.</param>
        /// <param name="queueMessage">String containing the context of the message.</param>
        /// <returns>The receipt from adding a message to the queue.</returns>
        public async Task<SendReceipt> ExecuteAsync(string queueName, string queueMessage)
        {
            await PreQueueChecksAsync(queueName);
            
            var response = await QueueClient.SendMessageAsync(queueMessage);

            return response.Value;
        }
    }
}