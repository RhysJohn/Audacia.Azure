using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Audacia.Azure.StorageQueue.Config;
using Audacia.Azure.StorageQueue.Models;
using Audacia.Azure.StorageQueue.Services.Interfaces;
using Azure.Storage.Queues;
using Microsoft.Extensions.Options;

namespace Audacia.Azure.StorageQueue.Services
{
    /// <summary>
    /// Get service for returning messages from an Azure Queue Storage account.
    /// </summary>
    public class GetAzureQueueStorageService : BaseQueueStorageService, IGetAzureQueueStorageService
    {
        /// <summary>
        /// Constructor option for when adding the <see cref="QueueClient"/> has being added to the DI.
        /// </summary>
        /// <param name="queueClient"></param>
        protected GetAzureQueueStorageService(QueueClient queueClient) : base(queueClient)
        {
        }

        /// <summary>
        /// Constructor option for using the Options pattern with <see cref="QueueStorageOption"/>. 
        /// </summary>
        /// <param name="queueStorageConfig"></param>
        protected GetAzureQueueStorageService(IOptions<QueueStorageOption> queueStorageConfig) : base(
            queueStorageConfig)
        {
        }

        /// <summary>
        /// Returns the top message from a queue.
        /// </summary>
        /// <param name="queueName">Name of the queue which you want to take the next message from.</param>
        /// <param name="deleteMessageAfterReceiving">
        /// Whether you want to remove the messages from the queue after they have been received.
        /// </param>
        /// <returns>The message from the queue if there is one.</returns>
        public async Task<AzureQueueStorageMessage> GetAsync(string queueName, bool deleteMessageAfterReceiving = true)
        {
            await PreQueueChecksAsync(queueName);

            var queueProperties = await QueueClient.GetPropertiesAsync();
            if (queueProperties.Value.ApproximateMessagesCount > 0)
            {
                var nextMessage = await QueueClient.ReceiveMessageAsync();

                if (deleteMessageAfterReceiving)
                {
                    await DeleteMessageAsync(nextMessage.Value);
                }

                if (nextMessage.Value.InsertedOn != null)
                    return new AzureQueueStorageMessage(nextMessage.Value.MessageId,
                        nextMessage.Value.PopReceipt,
                        nextMessage.Value.MessageText,
                        nextMessage.Value.InsertedOn.Value.DateTime,
                        DateTime.Now
                    );
            }

            return null;
        }

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
        public async Task<IEnumerable<AzureQueueStorageMessage>> GetSomeAsync(string queueName,
            int amountToReceive,
            bool deleteMessageAfterReceiving = true)
        {
            await PreQueueChecksAsync(queueName);

            var queueProperties = await QueueClient.GetPropertiesAsync();
            if (queueProperties.Value.ApproximateMessagesCount > 0)
            {
                var response = await QueueClient.ReceiveMessagesAsync(amountToReceive);
                var nextMessages = response.Value;
                if (deleteMessageAfterReceiving)
                {
                    foreach (var nextMessage in nextMessages)
                    {
                        await DeleteMessageAsync(nextMessage);
                    }
                }

                var allMessagesHaveInsertedOn = nextMessages.All(message => message.InsertedOn.HasValue);
                if (allMessagesHaveInsertedOn)
                    return nextMessages.Select(message => new AzureQueueStorageMessage(message.MessageId,
                        message.PopReceipt,
                        message.MessageText,
                        message.InsertedOn.Value.DateTime,
                        DateTime.Now
                    )).ToList();
            }

            return new List<AzureQueueStorageMessage>();
        }
    }
}