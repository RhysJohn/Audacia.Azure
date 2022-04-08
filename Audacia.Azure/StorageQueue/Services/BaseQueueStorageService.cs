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
    public class BaseQueueStorageService
    {
        private readonly string _storageAccountConnectionString =
            "DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}";

        private readonly string _storageAccountUrl = "https://{0}.queue.core.windows.net";

        private readonly string _accountName;

        protected string StorageAccountUrl => string.Format(_storageAccountUrl, _accountName);

        public readonly string StorageAccountConnectionString;

        protected QueueClient QueueClient;

        protected BaseQueueStorageService(QueueClient queueClient)
        {
            QueueClient = queueClient ?? throw StorageQueueConfigurationException.QueueClientNotConfigured();

            _accountName = queueClient.AccountName;
        }

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

        protected async Task PreQueueChecksAsync(string queueName)
        {
            QueueClient = new QueueClient(StorageAccountConnectionString, queueName);

            var queueExists = await QueueClient.ExistsAsync();

            if (!queueExists)
            {
                throw new QueueDoesNotExistException(queueName);
            }
        }

        protected async Task<bool> DeleteMessageAsync(QueueMessage message)
        {
            var receivedMessageId = message.MessageId;
            var receivedMessagePopReceipt = message.PopReceipt;

            var response = await QueueClient.DeleteMessageAsync(receivedMessageId, receivedMessagePopReceipt);

            return response.Status == 200; // might be 202 as might not execute
        }
    }
}