using System.Threading.Tasks;
using Audacia.Azure.BlobStorage.Commands.UpdateCommands;

namespace Audacia.Azure.BlobStorage.Services.Interfaces
{
    /// <summary>
    /// Interface for updating existing blobs stored within an Azure Storage account.
    /// </summary>
    public interface IUpdateAzureBlobStorageService
    {
        /// <summary>
        /// Update an existing blob with the file data from the file stored at <paramref name="command.FilePath"/>.
        /// </summary>
        /// <param name="command">Command request containing all the information to upload a blob.</param>
        /// <returns>A bool depending on the success of the upload</returns>
        Task<bool> ExecuteAsync(UpdateAzureBlobStorageFileCommand command);
        
        /// <summary>
        /// Update an existing blob with the file data contained within the <paramref name="command.FileData"/>.
        /// </summary>
        /// <param name="command">Command request containing all the information to upload a blob.</param>
        /// <returns>A bool depending on the success of the upload</returns>
        Task<bool> ExecuteAsync(UpdateAzureBlobStorageBytesCommand command);
        
        /// <summary>
        /// Update an existing blob with the file data contained within the <paramref name="command.FileData"/>.
        /// </summary>
        /// <param name="command">Command request containing all the information to upload a blob.</param>
        /// <returns>A bool depending on the success of the upload</returns>
        Task<bool> ExecuteAsync(UpdateAzureBlobStorageBase64Command command);
        
        /// <summary>
        /// Update an existing blob with the file data contained within the <paramref name="command.FileData"/>.
        /// </summary>
        /// <param name="command">Command request containing all the information to upload a blob.</param>
        /// <returns>A bool depending on the success of the upload</returns>
        Task<bool> ExecuteAsync(UpdateAzureBlobStorageStreamCommand command);
    }
}