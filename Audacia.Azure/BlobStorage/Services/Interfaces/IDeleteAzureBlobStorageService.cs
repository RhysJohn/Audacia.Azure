using System.Threading.Tasks;

namespace Audacia.Azure.BlobStorage.Services.Interfaces
{
    /// <summary>
    /// Interface for removing blobs from an Azure Storage account.
    /// </summary>
    public interface IDeleteAzureBlobStorageService
    {
        /// <summary>
        /// Removes a blob with the <paramref name="blobName"/> within <paramref name="containerName"/>.
        /// </summary>
        /// <param name="containerName">The name of the container where the blob is stored.</param>
        /// <param name="blobName">The name of the blob you are wanting to remove.</param>
        /// <returns>Whether the removing of the blob was successful.</returns>
        Task<bool> ExecuteAsync(string containerName, string blobName);
    }
}