using System.IO;
using System.Threading.Tasks;
using Audacia.Azure.BlobStorage.Commands.AddCommands;

namespace Audacia.Azure.BlobStorage.Services.Interfaces
{
    /// <summary>
    /// Interface for the adding blobs to an Azure Storage account.
    /// </summary>
    public interface IAddAzureBlobStorageService
    {
        /// <summary>
        /// Adds a blob to Azure Blob Storage account when you would like to upload a blob where the data is in base 64 format.
        /// </summary>
        /// <param name="command">Command request containing all the information to upload a blob.</param>
        /// <returns>A bool depending on the success of the upload</returns>
        Task<bool> ExecuteAsync(AddAzureBlobStorageBase64Command command);
        
        /// <summary>
        /// Adds a blob to Azure Blob Storage account when you would like to upload a blob where the data is located
        /// on the local file server.
        /// </summary>
        /// <param name="command">Command request containing all the information to upload a blob.</param>
        /// <returns>A bool depending on the success of the upload</returns>
        Task<bool> ExecuteAsync(AddAzureBlobStorageFileCommand command);

        /// <summary>
        /// Adds a blob to Azure Blob Storage account when you have a byte array containing the data you want to add to
        /// the storage account.
        /// </summary>
        /// <param name="command">Command request containing all the information to upload a blob.</param>
        /// <returns>A bool depending on the success of the upload</returns>
        Task<bool> ExecuteAsync(AddAzureBlobStorageBytesCommand command);
        
        /// <summary>
        /// Adds a blob to Azure Blob Storage account when you have a stream containing the data of the blob you want
        /// to upload to the storage account.
        /// </summary>
        /// <param name="command">Command request containing all the information to upload a blob.</param>
        /// <returns>A bool depending on the success of the upload</returns>
        Task<bool> ExecuteAsync(AddAzureBlobStorageStreamCommand command);
    }
}