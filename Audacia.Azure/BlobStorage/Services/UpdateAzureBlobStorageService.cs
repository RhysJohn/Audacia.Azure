using System;
using System.IO;
using System.Threading.Tasks;
using Audacia.Azure.BlobStorage.BaseServices;
using Audacia.Azure.BlobStorage.Config;
using Audacia.Azure.BlobStorage.Exceptions;
using Audacia.Azure.BlobStorage.Services.Interfaces;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;

namespace Audacia.Azure.BlobStorage.Services
{
    /// <summary>
    /// Update service for returning blob from an Azure Blob Storage account.
    /// </summary>
    public class AzureUpdateAzureBlobStorageService : BaseAzureUpdateStorageService, IUpdateAzureBlobStorageService
    {
        /// <summary>
        /// Constructor option for when adding the <see cref="BlobServiceClient"/> has being added to the DI.
        /// </summary>
        /// <param name="blobServiceClient"></param>
        public AzureUpdateAzureBlobStorageService(BlobServiceClient blobServiceClient) : base(blobServiceClient)
        {
        }

        /// <summary>
        /// Constructor option for using the Options pattern with <see cref="BlobStorageOption"/>. 
        /// </summary>
        /// <param name="blobStorageConfig"></param>
        public AzureUpdateAzureBlobStorageService(IOptions<BlobStorageOption> blobStorageConfig) : base(
            blobStorageConfig)
        {
        }

        /// <summary>
        /// Update an existing blob with the file data from the file stored at <paramref name="filePath"/>.
        /// </summary>
        /// <param name="containerName">The name of the container where you want the blob to be added</param>
        /// <param name="blobName">The name of the blob which is going to be added to the storage account</param>
        /// <param name="filePath">The full file path to the location of the file which you want to upload</param>
        /// <param name="doesContainerExist">Whether or not the container already exist</param>
        /// <returns>A bool depending on the success of the upload</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<bool> ExecuteAsync(string containerName, string blobName, string filePath,
            bool doesContainerExist = true)
        {
            PreContainerChecks(containerName, doesContainerExist);

            var container = GetContainer(containerName);

            if (container != null)
            {
                var fileData = GetFileData(filePath);

                var blobClient = container.GetBlobClient(blobName);

                var blobExists = await blobClient.ExistsAsync();

                if (blobExists.Value)
                {
                    await blobClient.DeleteAsync();

                    using (var ms = new MemoryStream(fileData, false))
                    {
                        var result = await blobClient.UploadAsync(ms);

                        // The result will be null if it failed
                        return result != null;
                    }
                }

                throw new BlobDoesNotExistException(blobName, containerName);
            }

            throw new ContainerDoesNotExistException(containerName);
        }

        /// <summary>
        /// Update an existing blob with the file data contained within the <paramref name="fileData"/>.
        /// </summary>
        /// <param name="containerName">The name of the container where you want the blob to be added</param>
        /// <param name="blobName">The name of the blob which is going to be added to the storage account</param>
        /// <param name="fileData">The full file path to the location of the file which you want to upload</param>
        /// <param name="doesContainerExist">Whether or not the container already exist</param>
        /// <returns>A bool depending on the success of the upload</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<bool> ExecuteAsync(string containerName, string blobName, byte[] fileData,
            bool doesContainerExist = true)
        {
            PreContainerChecks(containerName, doesContainerExist);

            var container = GetContainer(containerName);

            if (container != null)
            {
                var blobClient = container.GetBlobClient(blobName);

                var blobExists = await blobClient.ExistsAsync();

                if (blobExists.Value)
                {
                    await blobClient.DeleteAsync();

                    using (var ms = new MemoryStream(fileData, false))
                    {
                        var result = await blobClient.UploadAsync(ms);

                        // The result will be null if it failed
                        return result != null;
                    }
                }

                throw new BlobDoesNotExistException(blobName, containerName);
            }

            throw new ContainerDoesNotExistException(containerName);
        }
    }
}