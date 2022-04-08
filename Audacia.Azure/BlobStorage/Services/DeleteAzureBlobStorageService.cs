using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Audacia.Azure.BlobStorage.Commands.DeleteCommands;
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
    /// Delete service for removing blobs from an Azure Blob Storage account.
    /// </summary>
    public class DeleteAzureBlobStorageService : BaseAzureBlobStorageService, IDeleteAzureBlobStorageService
    {
        /// <summary>
        /// Constructor option for when adding the <see cref="BlobServiceClient"/> has being added to the DI.
        /// </summary>
        /// <param name="blobServiceClient"></param>
        public DeleteAzureBlobStorageService(BlobServiceClient blobServiceClient) : base(blobServiceClient)
        {
        }

        /// <summary>
        /// Constructor option for using the Options pattern with <see cref="BlobStorageOption"/>. 
        /// </summary>
        /// <param name="blobStorageConfig"></param>
        public DeleteAzureBlobStorageService(IOptions<BlobStorageOption> blobStorageConfig) : base(
            blobStorageConfig)
        {
        }

        /// <summary>
        /// Removes a blob with the <paramref name="command.BlobName"/> within <paramref name="command.ContainerName"/>.
        /// </summary>
        /// <param name="command">Command request containing all the information to remove a blob.</param>
        /// <returns>Whether the removing of the blob was successful.</returns>
        /// <exception cref="BlobDoesNotExistException"></exception>
        public async Task<bool> ExecuteAsync(DeleteAzureBlobStorageCommand command)
        {
            var containerClient = BlobServiceClient.GetBlobContainerClient(command.ContainerName);

            var blobClient = containerClient.GetBlobClient(command.BlobName);
            var blobExists = await blobClient.ExistsAsync();

            if (blobExists.Value)
            {
                try
                {
                    await blobClient.DeleteAsync();

                    return true;
                }
                catch (RequestFailedException _)
                {
                    return false;
                }
            }

            throw new BlobDoesNotExistException(command.BlobName, command.ContainerName);
        }
    }
}