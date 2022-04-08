using System;
using System.IO;
using System.Threading.Tasks;
using Audacia.Azure.BlobStorage.Commands.AddCommands;
using Audacia.Azure.BlobStorage.Config;
using Audacia.Azure.BlobStorage.Exceptions;
using Audacia.Azure.BlobStorage.Services.Base;
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

        public async Task<bool> ExecuteAsync(AddAzureBlobStorageBase64Command command)
        {
            ContainerChecks(command.ContainerName, command.DoesContainerExist);

            var container = await GetOrCreateContainerAsync(command.ContainerName, command.DoesContainerExist);

            if (container != null)
            {
                var blobData = Convert.FromBase64String(command.BlobData);

                return await UploadBlobToBlobStorageAsync(container, command, blobData);
            }

            return false;
        }

        /// <summary>
        /// Adds a blob to Azure Blob Storage account when you would like to upload a blob where the data is located
        /// on the local file server.
        /// </summary>
        /// <param name="command">Command request containing all the information to upload a blob.</param>
        /// <returns>A bool depending on the success of the upload</returns>
        /// <exception cref="BlobNameAlreadyExistsException"></exception>
        public async Task<bool> ExecuteAsync(AddAzureBlobStorageFileCommand command)
        {
            ContainerChecks(command.ContainerName, command.DoesContainerExist);

            var container = await GetOrCreateContainerAsync(command.ContainerName, command.DoesContainerExist);

            if (container != null)
            {
                var blobData = GetFileData(command.FilePath);

                return await UploadBlobToBlobStorageAsync(container, command, blobData);
            }

            return false;
        }

        /// <summary>
        /// Adds a blob to Azure Blob Storage account when you have a byte array containing the data you want to add to
        /// the storage account.
        /// </summary>
        /// <param name="command">Command request containing all the information to upload a blob.</param>
        /// <returns>A bool depending on the success of the upload</returns>
        /// <exception cref="BlobNameAlreadyExistsException"></exception>
        public async Task<bool> ExecuteAsync(AddAzureBlobStorageBytesCommand command)
        {
            ContainerChecks(command.ContainerName, command.DoesContainerExist);

            var container = await GetOrCreateContainerAsync(command.ContainerName, command.DoesContainerExist);

            if (container != null)
            {
                return await UploadBlobToBlobStorageAsync(container, command, command.BlobData);
            }

            return false;
        }

        /// <summary>
        /// Adds a blob to Azure Blob Storage account when you have a stream containing the data of the blob you want
        /// to upload to the storage account.
        /// </summary>
        /// <param name="command">Command request containing all the information to upload a blob.</param>
        /// <returns>A bool depending on the success of the upload</returns>
        /// <exception cref="BlobNameAlreadyExistsException"></exception>
        public async Task<bool> ExecuteAsync(AddAzureBlobStorageStreamCommand command)
        {
            ContainerChecks(command.ContainerName, command.DoesContainerExist);

            var container = await GetOrCreateContainerAsync(command.ContainerName, command.DoesContainerExist);

            if (container != null)
            {
                var blobClient = container.GetBlobClient(command.BlobName);

                var blobExists = await blobClient.ExistsAsync();

                if (!blobExists.Value)
                {
                    try
                    {
                        await blobClient.UploadAsync(command.BlobData);
                        return true;
                    }
                    catch (RequestFailedException _)
                    {
                        return false;
                    }
                }

                throw new BlobNameAlreadyExistsException(command.BlobName, command.ContainerName);
            }

            return false;
        }

        /// <summary>
        /// Adds a blob to Azure Blob Storage account.
        /// </summary>
        /// <param name="container">Container which the blob is going to be uploaded too.</param>
        /// <param name="command">Command request containing all the information to upload a blob.</param>
        /// <param name="blobData">Byte array of the blob being uploaded.</param>
        /// <returns>A bool depending on the success of the upload</returns>
        /// <exception cref="BlobNameAlreadyExistsException"></exception>
        private async Task<bool> UploadBlobToBlobStorageAsync(BlobContainerClient container,
            BaseAddBlobStorageCommand command, byte[] blobData)
        {
            var blobClient = container.GetBlobClient(command.BlobName);

            var blobExists = await blobClient.ExistsAsync();

            if (!blobExists.Value)
            {
                await using (var ms = new MemoryStream(blobData, false))
                {
                    try
                    {
                        await blobClient.UploadAsync(ms);
                        
                        return true;
                    }
                    catch (RequestFailedException _)
                    {
                        throw _;
                    }
                }
            }

            throw new BlobNameAlreadyExistsException(command.BlobName, command.ContainerName);
        }
    }
}