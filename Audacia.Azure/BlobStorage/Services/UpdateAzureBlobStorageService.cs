using System;
using System.IO;
using System.Threading.Tasks;
using Audacia.Azure.BlobStorage.Commands.UpdateCommands;
using Audacia.Azure.BlobStorage.Config;
using Audacia.Azure.BlobStorage.Exceptions;
using Audacia.Azure.BlobStorage.Exceptions.BlobDataExceptions;
using Audacia.Azure.BlobStorage.Models;
using Audacia.Azure.BlobStorage.Services.Base;
using Audacia.Azure.BlobStorage.Services.Interfaces;
using Azure;
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
        /// Update an existing blob with the file data from the file stored at <paramref name="command.FilePath"/>.
        /// </summary>
        /// <param name="command">Command request containing all the information to upload a blob.</param>
        /// <returns>A bool depending on the success of the upload</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<bool> ExecuteAsync(UpdateAzureBlobStorageFileCommand command)
        {
            ContainerChecks(command.ContainerName, command.DoesContainerExist);

            var blobData = FileLocationBlobChecks(command.BlobName, command.FilePath);

            var container = GetContainer(command.ContainerName);

            if (container != null)
            {
                return await UpdateBlobInBlobStorageAsync(container, command, blobData);
            }

            throw new ContainerDoesNotExistException(command.ContainerName);
        }

        private byte[] FileLocationBlobChecks(string blobName, string fileLocation)
        {
            if (fileLocation == null)
            {
                throw new BlobDataCannotBeNullException(blobName, BlobDataType.FileLocation);
            }

            if (fileLocation.Length <= 0)
            {
                throw new BlobDataCannotBeEmptyException(blobName, BlobDataType.FileLocation);
            }

            if (!File.Exists(fileLocation))
            {
                throw new BlobFileLocationNeedsToExistException(blobName, BlobDataType.FileLocation, fileLocation);
            }

            var blobData = GetFileData(fileLocation);

            return blobData;
        }

        /// <summary>
        /// Update an existing blob with the file data contained within the <paramref name="command.FileData"/>.
        /// </summary>
        /// <param name="command">Command request containing all the information to upload a blob.</param>
        /// <returns>A bool depending on the success of the upload</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<bool> ExecuteAsync(UpdateAzureBlobStorageBytesCommand command)
        {
            ContainerChecks(command.ContainerName, command.DoesContainerExist);

            var blobData = BytesBlobChecks(command.BlobName, command.BlobData);

            var container = GetContainer(command.ContainerName);

            if (container != null)
            {
                return await UpdateBlobInBlobStorageAsync(container, command, blobData);
            }

            throw new ContainerDoesNotExistException(command.ContainerName);
        }

        private byte[] BytesBlobChecks(string blobName, byte[] bytesBlobData)
        {
            if (bytesBlobData == null)
            {
                throw new BlobDataCannotBeNullException(blobName, BlobDataType.ByteArray);
            }

            if (bytesBlobData.Length <= 0)
            {
                throw new BlobDataCannotBeEmptyException(blobName, BlobDataType.ByteArray);
            }

            return bytesBlobData;
        }

        /// <summary>
        /// Update an existing blob with the file data contained within the <paramref name="command.FileData"/>.
        /// </summary>
        /// <param name="command">Command request containing all the information to upload a blob.</param>
        /// <returns>A bool depending on the success of the upload</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<bool> ExecuteAsync(UpdateAzureBlobStorageBase64Command command)
        {
            ContainerChecks(command.ContainerName, command.DoesContainerExist);

            var blobData = Base64BlobChecks(command.BlobName, command.BlobData);

            var container = GetContainer(command.ContainerName);

            if (container != null)
            {
                return await UpdateBlobInBlobStorageAsync(container, command, blobData);
            }

            throw new ContainerDoesNotExistException(command.ContainerName);
        }

        private byte[] Base64BlobChecks(string blobName, string base64BlobData)
        {
            if (base64BlobData == null)
            {
                throw new BlobDataCannotBeNullException(blobName, BlobDataType.Base64);
            }

            if (base64BlobData.Length <= 0)
            {
                throw new BlobDataCannotBeEmptyException(blobName, BlobDataType.Base64);
            }

            var buffer = new Span<byte>(new byte[base64BlobData.Length]);
            var isValidBase64 = Convert.TryFromBase64String(base64BlobData, buffer, out _);

            if (isValidBase64)
            {
                throw new BlobDataCannotBeInvalidBase64Exception(blobName, base64BlobData);
            }

            return buffer.ToArray();
        }

        /// <summary>
        /// Update an existing blob with the file data contained within the <paramref name="command.FileData"/>.
        /// </summary>
        /// <param name="command">Command request containing all the information to upload a blob.</param>
        /// <returns>A bool depending on the success of the upload</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<bool> ExecuteAsync(UpdateAzureBlobStorageStreamCommand command)
        {
            ContainerChecks(command.ContainerName, command.DoesContainerExist);

            var blobData = StreamBlobDataCheck(command.BlobName, command.BlobData);

            var container = GetContainer(command.ContainerName);

            if (container != null)
            {
                var blobClient = container.GetBlobClient(command.BlobName);

                var blobExists = await blobClient.ExistsAsync();

                if (blobExists.Value)
                {
                    await blobClient.DeleteAsync();

                    try
                    {
                        await blobClient.UploadAsync(blobData);
                        return true;
                    }
                    catch (RequestFailedException _)
                    {
                        return false;
                    }
                }

                throw new BlobDoesNotExistException(command.BlobName, command.ContainerName);
            }

            throw new ContainerDoesNotExistException(command.ContainerName);
        }

        private Stream StreamBlobDataCheck(string blobName, Stream streamBlobData)
        {
            if (streamBlobData == null)
            {
                throw new BlobDataCannotBeNullException(blobName, BlobDataType.Stream);
            }

            if (streamBlobData.Length <= 0)
            {
                throw new BlobDataCannotBeEmptyException(blobName, BlobDataType.Stream);
            }

            return streamBlobData;
        }

        private async Task<bool> UpdateBlobInBlobStorageAsync(BlobContainerClient container,
            BaseUpdateBlobStorageCommand command, byte[] blobData)
        {
            var blobClient = container.GetBlobClient(command.BlobName);

            var blobExists = await blobClient.ExistsAsync();

            if (blobExists.Value)
            {
                await blobClient.DeleteAsync();

                await using (var ms = new MemoryStream(blobData, false))
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

            throw new BlobDoesNotExistException(command.BlobName, command.ContainerName);
        }
    }
}