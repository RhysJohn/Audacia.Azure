using System;
using System.Threading.Tasks;
using Audacia.Azure.BlobStorage.Config;
using Audacia.Azure.BlobStorage.Extensions;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;

namespace Audacia.Azure.BlobStorage.BaseServices
{
    public abstract class BaseAzureBlobStorageService
    {
        private readonly string _storageAccountConnectionString =
            "DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}";

        private readonly string _storageAccountUrl = "https://{0}.blob.core.windows.net";

        private readonly string _accountName;

        public string StorageAccountUrl => string.Format(_storageAccountUrl, _accountName);

        protected readonly BlobServiceClient BlobServiceClient;

        protected BaseAzureBlobStorageService(BlobServiceClient blobServiceClient)
        {
            if (blobServiceClient == null)
            {
                throw new Exception("Need to add BlobServiceClient to the service collection");
            }

            BlobServiceClient = blobServiceClient;

            _accountName = blobServiceClient.AccountName;
        }

        protected BaseAzureBlobStorageService(IOptions<BlobStorageOption> blobStorageConfig)
        {
            if (blobStorageConfig == null)
            {
                throw new Exception("Need to add a value of IOptions<BlobStorageConfig> to the DI");
            }

            if (string.IsNullOrEmpty(blobStorageConfig.Value.AccountName))
            {
                throw new Exception("Cannot connect to an Azure Blob storage with an null/empty account name");
            }

            if (string.IsNullOrEmpty(blobStorageConfig.Value.AccountKey))
            {
                throw new Exception("Cannot connect to an Azure Blob storage with an null/empty account key");
            }

            var storageAccountConnectionString = string.Format(_storageAccountConnectionString,
                blobStorageConfig.Value.AccountName, blobStorageConfig.Value.AccountKey);

            BlobServiceClient = new BlobServiceClient(storageAccountConnectionString);

            _accountName = blobStorageConfig.Value.AccountName;
        }

        protected BlobContainerClient GetContainer(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new Exception("Cannot find a blob container with a name that is null / empty");
            }

            return BlobServiceClient.GetBlobContainerClient(containerName);
        }

        protected async Task<BlobContainerClient> CreateContainerAsync(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new Exception("Cannot create a new container with a name that is null / empty");
            }

            return await BlobServiceClient.CreateBlobContainerAsync(containerName);
        }

        /// <summary>
        /// Checks if the container is pre existing.
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="doesContainerExist"></param>
        /// <exception cref="Exception"></exception>
        protected void PreContainerChecks(string containerName, bool doesContainerExist)
        {
            var storageAccountContainers = BlobServiceClient.GetBlobContainers();

            var checkContainerExists = storageAccountContainers.AlreadyExists(containerName);
            if (doesContainerExist && !checkContainerExists)
            {
                throw new Exception($"There is no container with the name: {containerName}");
            }

            // We should check that there is no containers already existing with the name passed in.
            if (!doesContainerExist && checkContainerExists)
            {
                throw new Exception(
                    $"There is already a container on this storage account with the name: {containerName}");
            }
        }
    }
}