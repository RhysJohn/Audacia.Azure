using System.Threading.Tasks;
using Audacia.Azure.BlobStorage.Commands.DeleteCommands;

namespace Audacia.Azure.BlobStorage.Services.Interfaces
{
    /// <summary>
    /// Interface for removing blobs from an Azure Storage account.
    /// </summary>
    public interface IDeleteAzureBlobStorageService
    {
        /// <summary>
        /// Removes a blob with the <paramref name="command.BlobName"/> within <paramref name="command.ContainerName"/>.
        /// </summary>
        /// <param name="command">Command request containing all the information to remove a blob.</param>
        /// <returns>Whether the removing of the blob was successful.</returns>
        Task<bool> ExecuteAsync(DeleteAzureBlobStorageCommand command);
    }
}