using System;
using System.Collections.Generic;
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
    /// Delete service for removing blobs from an Azure Blob Storage account.
    /// </summary>
    public class DeleteAzureAzureBlobStorageService : BaseAzureBlobStorageService, IDeleteAzureBlobStorageService
    {
        /// <summary>
        /// Constructor option for when adding the <see cref="BlobServiceClient"/> has being added to the DI.
        /// </summary>
        /// <param name="blobServiceClient"></param>
        public DeleteAzureAzureBlobStorageService(BlobServiceClient blobServiceClient) : base(blobServiceClient)
        {
        }

        /// <summary>
        /// Constructor option for using the Options pattern with <see cref="BlobStorageOption"/>. 
        /// </summary>
        /// <param name="blobStorageConfig"></param>
        public DeleteAzureAzureBlobStorageService(IOptions<BlobStorageOption> blobStorageConfig) : base(
            blobStorageConfig)
        {
        }

        /// <summary>
        /// Removes a blob with the <paramref name="blobName"/> within <paramref name="containerName"/>.
        /// </summary>
        /// <param name="containerName">The name of the container where the blob is stored.</param>
        /// <param name="blobName">The name of the blob you are wanting to remove.</param>
        /// <returns>Whether the removing of the blob was successful.</returns>
        /// <exception cref="BlobDoesNotExistException"></exception>
        public async Task<bool> ExecuteAsync(string containerName, string blobName)
        {
            var containerClient = BlobServiceClient.GetBlobContainerClient(containerName);

            var blobClient = containerClient.GetBlobClient(blobName);
            var blobExists = await blobClient.ExistsAsync();

            if (blobExists.Value)
            {
                // Delete the blob from the container
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

            throw new BlobDoesNotExistException(blobName, containerName);
        }
    }
}