using System.Threading.Tasks;

namespace Audacia.Azure.BlobStorage.Services.Interfaces
{
    /// <summary>
    /// Interface for updating existing blobs stored within an Azure Storage account.
    /// </summary>
    public interface IUpdateAzureBlobStorageService
    {
        
        /// <summary>
        /// Update an existing blob with the file data from the file stored at <paramref name="filePath"/>.
        /// </summary>
        /// <param name="containerName">The name of the container where you want the blob to be added</param>
        /// <param name="blobName">The name of the blob which is going to be added to the storage account</param>
        /// <param name="filePath">The full file path to the location of the file which you want to upload</param>
        /// <param name="doesContainerExist">Whether or not the container already exist</param>
        /// <returns>A bool depending on the success of the upload</returns>
        Task<bool> ExecuteAsync(string containerName, string blobName, string filePath, bool doesContainerExist = true);
        
        /// <summary>
        /// Update an existing blob with the file data contained within the <paramref name="fileData"/>.
        /// </summary>
        /// <param name="containerName">The name of the container where you want the blob to be added</param>
        /// <param name="blobName">The name of the blob which is going to be added to the storage account</param>
        /// <param name="fileData">The full file path to the location of the file which you want to upload</param>
        /// <param name="doesContainerExist">Whether or not the container already exist</param>
        /// <returns>A bool depending on the success of the upload</returns>
        Task<bool> ExecuteAsync(string containerName, string blobName, byte[] fileData, bool doesContainerExist = true);
    }
}