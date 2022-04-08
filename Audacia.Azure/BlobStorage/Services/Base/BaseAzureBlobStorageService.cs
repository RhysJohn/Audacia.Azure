using System.Threading.Tasks;
using Audacia.Azure.BlobStorage.Config;
using Audacia.Azure.BlobStorage.Exceptions;
using Audacia.Azure.BlobStorage.Extensions;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;

namespace Audacia.Azure.BlobStorage.Services.Base
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
            BlobServiceClient = blobServiceClient ?? throw BlobStorageConfigurationException.BlobClientNotConfigured();

            _accountName = blobServiceClient.AccountName;
        }

        protected BaseAzureBlobStorageService(IOptions<BlobStorageOption> blobStorageConfig)
        {
            if (blobStorageConfig?.Value == null)
            {
                throw BlobStorageConfigurationException.OptionsNotConfigured();
            }

            if (string.IsNullOrEmpty(blobStorageConfig.Value.AccountName))
            {
                throw BlobStorageConfigurationException.AccountNameNotConfigured();
            }

            if (string.IsNullOrEmpty(blobStorageConfig.Value.AccountKey))
            {
                throw BlobStorageConfigurationException.AccountKeyNotConfigured();
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
                throw ContainerNameInvalidException.UnableToFindWithContainerName();
            }

            return BlobServiceClient.GetBlobContainerClient(containerName);
        }

        protected async Task<BlobContainerClient> CreateContainerAsync(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw ContainerNameInvalidException.UnableToCreateWithContainerName(containerName);
            }

            return await BlobServiceClient.CreateBlobContainerAsync(containerName);
        }

        /// <summary>
        /// Checks if the container is pre existing.
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="doesContainerExist"></param>
        /// <exception cref="ContainerDoesNotExistException"></exception>
        /// <exception cref="ContainerAlreadyExistsException"></exception>
        protected void ContainerChecks(string containerName, bool doesContainerExist)
        {
            var storageAccountContainers = BlobServiceClient.GetBlobContainers();

            var checkContainerExists = storageAccountContainers.AlreadyExists(containerName);
            if (doesContainerExist && !checkContainerExists)
            {
                throw new ContainerDoesNotExistException(containerName);
            }

            // We should check that there is no containers already existing with the name passed in.
            if (!doesContainerExist && checkContainerExists)
            {
                throw new ContainerAlreadyExistsException(containerName);
            }
        }
    }
}