using System.IO;
using System.Threading.Tasks;

namespace Audacia.Azure.BlobStorage.Services.Interfaces
{
    /// <summary>
    /// Interface for the adding blobs to an Azure Storage account.
    /// </summary>
    public interface IAddAzureBlobStorageService
    {
        /// <summary>
        /// Adds a blob to Azure Blob Storage account when you would like to upload a blob where the data is located
        /// on the local file server.
        /// </summary>
        /// <param name="containerName">The name of the container where you want the blob to be added</param>
        /// <param name="blobName">The name of the blob which is going to be added to the storage account</param>
        /// <param name="filePath">The full file path to the location of the file which you want to upload</param>
        /// <param name="doesContainerExist">Whether or not the container already exist</param>
        /// <returns>A bool depending on the success of the upload</returns>
        Task<bool> ExecuteAsync(string containerName, string blobName, string filePath, bool doesContainerExist = true);

        /// <summary>
        /// Adds a blob to Azure Blob Storage account when you have a byte array containing the data you want to add to
        /// the storage account.
        /// </summary>
        /// <param name="containerName">The name of the container where you want the blob to be added</param>
        /// <param name="blobName">The name of the blob which is going to be added to the storage account</param>
        /// <param name="fileData">Byte array of the data which you want to upload to the storage account</param>
        /// <param name="doesContainerExist">Whether or not the container already exist</param>
        /// <returns>A bool depending on the success of the upload</returns>
        Task<bool> ExecuteAsync(string containerName, string blobName, byte[] fileData, bool doesContainerExist = true);
        
        /// <summary>
        /// Adds a blob to Azure Blob Storage account when you have a stream containing the data of the blob you want
        /// to upload to the storage account.
        /// </summary>
        /// <param name="containerName">The name of the container where you want the blob to be added</param>
        /// <param name="blobName">The name of the blob which is going to be added to the storage account</param>
        /// <param name="fileData">A stream of the data of the blob to be added to the storage account</param>
        /// <param name="doesContainerExist">Whether or not the container already exist</param>
        /// <returns>A bool depending on the success of the upload</returns>
        Task<bool> ExecuteAsync(string containerName, string blobName, Stream fileData, bool doesContainerExist = true);
    }
}