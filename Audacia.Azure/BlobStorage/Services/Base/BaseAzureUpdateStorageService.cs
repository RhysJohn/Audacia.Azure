using System.IO;
using System.Threading.Tasks;
using Audacia.Azure.BlobStorage.Config;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;

namespace Audacia.Azure.BlobStorage.Services.Base
{
    /// <summary>
    /// Base class for adding and updating blobs.
    /// </summary>
    public abstract class BaseAzureUpdateStorageService : BaseAzureBlobStorageService
    {
        protected BaseAzureUpdateStorageService(BlobServiceClient blobServiceClient) : base(blobServiceClient)
        {
        }

        protected BaseAzureUpdateStorageService(IOptions<BlobStorageOption> blobStorageConfig) : base(blobStorageConfig)
        {
        }

        /// <summary>
        /// Gets the byte array containing the data from a file stored on the local file storage.
        /// </summary>
        /// <param name="filePath">Full file path for where a file is located.</param>
        /// <returns>Byte array containing the data from the file stored at <paramref name="filePath"/>.</returns>
        /// <exception cref="FileNotFoundException">Thrown when unable to find the file located at <paramref name="filePath"/>.</exception>
        protected byte[] GetFileData(string filePath)
        {
            var fileExist = File.Exists(filePath);

            if (fileExist)
            {
                var bytes = File.ReadAllBytes(filePath);

                return bytes;
            }

            throw new FileNotFoundException($"File does not exist at: {filePath}");
        }

        /// <summary>
        /// Gets or creates a <see cref="BlobContainerClient"/>.
        /// </summary>
        /// <param name="containerName">The name of the container</param>
        /// <param name="doesContainerExist">Whether or not you expect the container to exist.</param>
        /// <returns>Returns the existing or newly created <see cref="BlobContainerClient"/>.</returns>
        protected async Task<BlobContainerClient> GetOrCreateContainerAsync(string containerName,
            bool doesContainerExist)
        {
            BlobContainerClient container;
            if (!doesContainerExist)
            {
                container = await CreateContainerAsync(containerName);
            }
            else
            {
                container = GetContainer(containerName);
            }

            return container;
        }
    }
}