using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Audacia.Azure.BlobStorage.Config;
using Audacia.Azure.BlobStorage.Exceptions;
using Audacia.Azure.BlobStorage.Services.Base;
using Audacia.Azure.BlobStorage.Services.Interfaces;
using Audacia.Azure.ReturnOptions;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;

namespace Audacia.Azure.BlobStorage.Services
{
    /// <summary>
    /// Get service for returning blob from an Azure Blob Storage account.
    /// </summary>
    public class GetAzureAzureBlobStorageService : BaseAzureBlobStorageService, IGetAzureBlobStorageService
    {
        private string StorageAccountWithContainer => $"{StorageAccountUrl}/{{0}}";

        /// <summary>
        /// Constructor option for when adding the <see cref="BlobServiceClient"/> has being added to the DI.
        /// </summary>
        /// <param name="blobServiceClient"></param>
        public GetAzureAzureBlobStorageService(BlobServiceClient blobServiceClient) : base(blobServiceClient)
        {
        }

        /// <summary>
        /// Constructor option for using the Options pattern with <see cref="BlobStorageOption"/>. 
        /// </summary>
        /// <param name="blobStorageConfig"></param>
        public GetAzureAzureBlobStorageService(IOptions<BlobStorageOption> blobStorageConfig) : base(blobStorageConfig)
        {
        }

        /// <summary>
        /// Returns a blob with the name <paramref name="blobName"/> within the <paramref name="containerName"/>.
        /// </summary>
        /// <param name="containerName">The name of the container where the blob you want to return is stored in.</param>
        /// <param name="blobName">The name of the blob you are wanting to return.</param>
        /// <typeparam name="T">The type of data which you want the blob to be converted into. This must match one of the
        /// return options. Please look into the different return options to decide which is best suited for you.</typeparam>
        /// <typeparam name="TResponse">The return option which you want the blob to be returned in.</typeparam>
        /// <returns>A <see cref="IBlobReturnOption"/> which has been configured by the generic arguments.</returns>
        public async Task<T> GetAsync<T, TResponse>(string containerName, string blobName)
            where TResponse : IBlobReturnOption<T>, new()
        {
            var containers = BlobServiceClient.GetBlobContainers();
            var containerExists = containers.Any(container => container.Name == containerName);

            if (containerExists)
            {
                var containerClient = BlobServiceClient.GetBlobContainerClient(containerName);

                var blobBytes = await GetBlobBytesAsync(containerClient, blobName);

                return new TResponse().Parse(blobName, blobBytes,
                    string.Format(StorageAccountWithContainer, containerName));
            }

            throw new ContainerDoesNotExistException(containerName);
        }

        /// <summary>
        /// Returns a collection of blob based on the collection of blob names <paramref name="blobNames"/> within the
        /// container within an Azure storage account.
        /// </summary>
        /// <param name="containerName">The name of the container where the blob you want to return is stored in.</param>
        /// <param name="blobNames">A collection of blob names you are wanting to return.</param>
        /// <typeparam name="T">The type of data which you want the blob to be converted into. This must match one of the
        /// return options. Please look into the different return options to decide which is best suited for you.</typeparam>
        /// <typeparam name="TResponse">The return option which you want the blob to be returned in.</typeparam>
        /// <returns>A <see cref="IBlobReturnOption"/> which has been configured by the generic arguments.</returns>
        public async Task<IDictionary<string, T>> GetSomeAsync<T, TResponse>(string containerName,
            IEnumerable<string> blobNames)
            where TResponse : IBlobReturnOption<T>, new()
        {
            var containers = BlobServiceClient.GetBlobContainers();
            var containerExists = containers.Any(container => container.Name == containerName);

            if (containerExists)
            {
                var containerClient = BlobServiceClient.GetBlobContainerClient(containerName);

                var blobBytesDictionary = new Dictionary<string, T>();
                foreach (var blobName in blobNames)
                {
                    var blobBytes = await GetBlobBytesAsync(containerClient, blobName);
                    var parsedResult = new TResponse().Parse(blobName, blobBytes,
                        string.Format(StorageAccountWithContainer, containerName));

                    blobBytesDictionary.Add(blobName, parsedResult);
                }

                return blobBytesDictionary;
            }

            throw new ContainerDoesNotExistException(containerName);
        }

        /// <summary>
        /// Returns all the blob within a container within the storage account.
        /// </summary>
        /// <param name="containerName">The name of the container where the blobs you want to return are stored in.</param>
        /// <typeparam name="T">The type of data which you want the blob to be converted into. This must match one of the
        /// return options. Please look into the different return options to decide which is best suited for you.</typeparam>
        /// <typeparam name="TResponse">The return option which you want the blob to be returned in.</typeparam>
        /// <returns>A collection of <see cref="IBlobReturnOption"/> which has been configured by the generic arguments.</returns>
        public async Task<IDictionary<string, T>> GetAllAsync<T, TResponse>(string containerName)
            where TResponse : IBlobReturnOption<T>, new()
        {
            var containers = BlobServiceClient.GetBlobContainers();
            var containerExists = containers.Any(container => container.Name == containerName);

            if (containerExists)
            {
                var containerClient = BlobServiceClient.GetBlobContainerClient(containerName);
                var pagedBlobs = containerClient.GetBlobs();

                var blobs = pagedBlobs.ToList();

                var blobBytesDictionary = new Dictionary<string, T>();

                foreach (var blob in blobs)
                {
                    var blobBytes = await GetBlobBytesAsync(containerClient, blob.Name);
                    var parsedResult = new TResponse().Parse(blob.Name, blobBytes,
                        string.Format(StorageAccountWithContainer, containerName));

                    blobBytesDictionary.Add(blob.Name, parsedResult);
                }

                return blobBytesDictionary;
            }

            throw new ContainerDoesNotExistException(containerName);
        }

        /// <summary>
        /// Downloads the blob from the storage account and returns a convert byte array of the blobs data.
        /// </summary>
        /// <param name="containerClient">Name of the container where the blob is stored within.</param>
        /// <param name="blobName">Name of the blob which is going to be downloaded from the storage account.</param>
        /// <returns>Byte array of the data of the blob.</returns>
        private async Task<byte[]> GetBlobBytesAsync(BlobContainerClient containerClient, string blobName)
        {
            var blobClient = containerClient.GetBlobClient(blobName);
            var blobDownloadInfo = await blobClient.DownloadAsync();

            byte[] blobBytes;
            using (var memoryStream = new MemoryStream())
            {
                await blobDownloadInfo.Value.Content.CopyToAsync(memoryStream);

                blobBytes = memoryStream.ToArray();

                await memoryStream.DisposeAsync();
            }

            return blobBytes;
        }
    }
}