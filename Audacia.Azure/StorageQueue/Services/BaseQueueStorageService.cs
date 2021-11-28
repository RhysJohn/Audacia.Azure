using System;
using System.Threading.Tasks;
using Audacia.Azure.BlobStorage.Config;
using Audacia.Azure.StorageQueue.Config;
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
            QueueClient = queueClient ?? throw new Exception("Need to add QueueClient to the service collection");

            _accountName = queueClient.AccountName;
        }

        protected BaseQueueStorageService(IOptions<QueueStorageOption> queueStorageConfig)
        {
            if (queueStorageConfig == null)
            {
                throw new Exception("Need to add a value of IOptions<QueueStorageOption> to the DI");
            }

            if (string.IsNullOrEmpty(queueStorageConfig.Value.AccountName))
            {
                throw new Exception("Cannot connect to an Azure Queue storage with an null/empty account name");
            }

            if (string.IsNullOrEmpty(queueStorageConfig.Value.AccountKey))
            {
                throw new Exception("Cannot connect to an Azure Queue storage with an null/empty account key");
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
                throw new Exception($"There is no queue with the name: {queueName}");
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