using System;
using System.Threading.Tasks;
using Audacia.Azure.BlobStorage.Config;
using Audacia.Azure.StorageQueue.Config;
using Audacia.Azure.StorageQueue.Exceptions;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Options;

namespace Audacia.Azure.StorageQueue.Services
{
    /// <summary>
    /// Base service for Azure queue storage.
    /// </summary>
    public class BaseQueueStorageService
    {
        private readonly string _storageAccountConnectionString =
            "DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}";

        private readonly string _storageAccountUrl = "https://{0}.queue.core.windows.net";

        private readonly string _accountName;

        protected string StorageAccountUrl => string.Format(_storageAccountUrl, _accountName);

        public readonly string StorageAccountConnectionString;

        protected QueueClient QueueClient;

        /// <summary>
        /// Client variation of the Constructor.
        /// </summary>
        /// <param name="queueClient"><see cref="QueueClient"/> from Ioc</param>
        /// <exception cref="StorageQueueConfigurationException">If <see cref="QueueClient"/> has not been configured.</exception>
        protected BaseQueueStorageService(QueueClient queueClient)
        {
            QueueClient = queueClient ?? throw StorageQueueConfigurationException.QueueClientNotConfigured();

            _accountName = queueClient.AccountName;
        }

        /// <summary>
        /// Options variation of the Constructor.
        /// </summary>
        /// <param name="queueStorageConfig">Options of <see cref="QueueStorageOption"/>.</param>
        /// <exception cref="StorageQueueConfigurationException">
        /// Exception if there is a miss configuration with <see cref="QueueStorageOption"/>.
        /// </exception>
        protected BaseQueueStorageService(IOptions<QueueStorageOption> queueStorageConfig)
        {
            if (queueStorageConfig?.Value == null)
            {
                throw StorageQueueConfigurationException.OptionsNotConfigured();
            }

            if (string.IsNullOrEmpty(queueStorageConfig.Value.AccountName))
            {
                throw StorageQueueConfigurationException.AccountNameNotConfigured();
            }

            if (string.IsNullOrEmpty(queueStorageConfig.Value.AccountKey))
            {
                throw StorageQueueConfigurationException.AccountKeyNotConfigured();
            }

            _accountName = queueStorageConfig.Value.AccountName;

            StorageAccountConnectionString = string.Format(_storageAccountConnectionString,
                queueStorageConfig.Value.AccountName, queueStorageConfig.Value.AccountKey);
        }

        /// <summary>
        /// Checks on the queue before any actions are done.
        /// </summary>
        /// <param name="queueName">Name of the queue you are checking</param>
        /// <exception cref="QueueDoesNotExistException">
        /// If the queue you are wanting to do something with, does not exist.
        /// </exception>
        protected async Task PreQueueChecksAsync(string queueName)
        {
            QueueClient = new QueueClient(StorageAccountConnectionString, queueName);

            var queueExists = await QueueClient.ExistsAsync();

            if (!queueExists)
            {
                throw new QueueDoesNotExistException(queueName);
            }
        }

        /// <summary>
        /// Deletes a message from a <see cref="QueueClient"/>.
        /// </summary>
        /// <param name="message"><see cref="QueueMessage"/> to be removed from the <see cref="QueueClient"/>.</param>
        /// <returns>Whether the message has been removed.</returns>
        protected async Task<bool> DeleteMessageAsync(QueueMessage message)
        {
            var receivedMessageId = message.MessageId;
            var receivedMessagePopReceipt = message.PopReceipt;

            var response = await QueueClient.DeleteMessageAsync(receivedMessageId, receivedMessagePopReceipt);

            return response.Status == 200; // might be 202 as might not execute
        }
    }
}