using System;
using System.IO;
using System.Threading.Tasks;
using Audacia.Azure.BlobStorage.BaseServices;
using Audacia.Azure.BlobStorage.Config;
using Audacia.Azure.BlobStorage.Exceptions;
using Audacia.Azure.BlobStorage.Services.Interfaces;
using Azure;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;

namespace Audacia.Azure.BlobStorage.Services
{
    /// <summary>
    /// Add service for uploading blobs to an Azure Blob Storage account.
    /// </summary>
    public class AddAzureBlobStorageService : BaseAzureUpdateStorageService, IAddAzureBlobStorageService
    {
        /// <summary>
        /// Constructor option for when adding the <see cref="BlobServiceClient"/> has being added to the DI.
        /// </summary>
        /// <param name="blobServiceClient"></param>
        public AddAzureBlobStorageService(BlobServiceClient blobServiceClient) : base(blobServiceClient)
        {
        }

        /// <summary>
        /// Constructor option for using the Options pattern with <see cref="BlobStorageOption"/>. 
        /// </summary>
        /// <param name="blobStorageConfig"></param>
        public AddAzureBlobStorageService(IOptions<BlobStorageOption> blobStorageConfig) : base(blobStorageConfig)
        {
        }

        /// <summary>
        /// Adds a blob to Azure Blob Storage account when you would like to upload a blob where the data is located
        /// on the local file server.
        /// </summary>
        /// <param name="containerName">The name of the container where you want the blob to be added</param>
        /// <param name="blobName">The name of the blob which is going to be added to the storage account</param>
        /// <param name="filePath">The full file path to the location of the file which you want to upload</param>
        /// <param name="doesContainerExist">Whether or not the container already exist</param>
        /// <returns>A bool depending on the success of the upload</returns>
        /// <exception cref="BlobNameAlreadyExistsException"></exception>
        public async Task<bool> ExecuteAsync(string containerName, string blobName, string filePath,
            bool doesContainerExist = true)
        {
            PreContainerChecks(containerName, doesContainerExist);

            var container = await GetOrCreateContainerAsync(containerName, doesContainerExist);

            if (container != null)
            {
                var fileData = GetFileData(filePath);

                var blobClient = container.GetBlobClient(blobName);

                var blobExists = await blobClient.ExistsAsync();

                if (!blobExists.Value)
                {
                    await using (var ms = new MemoryStream(fileData, false))
                    {
                        try
                        {
                            await blobClient.UploadAsync(ms);
                            return true;
                        }
                        catch (RequestFailedException _)
                        {
                            return false;
                        }
                    }
                }

                throw new BlobNameAlreadyExistsException(blobName, containerName);
            }

            return false;
        }

        /// <summary>
        /// Adds a blob to Azure Blob Storage account when you have a byte array containing the data you want to add to
        /// the storage account.
        /// </summary>
        /// <param name="containerName">The name of the container where you want the blob to be added</param>
        /// <param name="blobName">The name of the blob which is going to be added to the storage account</param>
        /// <param name="fileData">Byte array of the data which you want to upload to the storage account</param>
        /// <param name="doesContainerExist">Whether or not the container already exist</param>
        /// <returns>A bool depending on the success of the upload</returns>
        /// <exception cref="BlobNameAlreadyExistsException"></exception>
        public async Task<bool> ExecuteAsync(string containerName, string blobName, byte[] fileData,
            bool doesContainerExist = true)
        {
            PreContainerChecks(containerName, doesContainerExist);

            var container = await GetOrCreateContainerAsync(containerName, doesContainerExist);

            if (container != null)
            {
                var blobClient = container.GetBlobClient(blobName);

                var blobExists = await blobClient.ExistsAsync();

                if (!blobExists.Value)
                {
                    await using (var ms = new MemoryStream(fileData, false))
                    {
                        try
                        {
                            await blobClient.UploadAsync(ms);
                            return true;
                        }
                        catch (RequestFailedException _)
                        {
                            return false;
                        }
                    }
                }

                throw new BlobNameAlreadyExistsException(blobName, containerName);
            }

            return false;
        }

        /// <summary>
        /// Adds a blob to Azure Blob Storage account when you have a stream containing the data of the blob you want
        /// to upload to the storage account.
        /// </summary>
        /// <param name="containerName">The name of the container where you want the blob to be added</param>
        /// <param name="blobName">The name of the blob which is going to be added to the storage account</param>
        /// <param name="fileData">A stream of the data of the blob to be added to the storage account</param>
        /// <param name="doesContainerExist">Whether or not the container already exist</param>
        /// <returns>A bool depending on the success of the upload</returns>
        /// <exception cref="BlobNameAlreadyExistsException"></exception>
        public async Task<bool> ExecuteAsync(string containerName, string blobName, Stream fileData,
            bool doesContainerExist = true)
        {
            PreContainerChecks(containerName, doesContainerExist);

            var container = await GetOrCreateContainerAsync(containerName, doesContainerExist);

            if (container != null)
            {
                var blobClient = container.GetBlobClient(blobName);

                var blobExists = await blobClient.ExistsAsync();

                if (!blobExists.Value)
                {
                    try
                    {
                        await blobClient.UploadAsync(fileData);
                        return true;
                    }
                    catch (RequestFailedException _)
                    {
                        return false;
                    }
                }

                throw new BlobNameAlreadyExistsException(blobName, containerName);
            }

            return false;
        }
    }
}