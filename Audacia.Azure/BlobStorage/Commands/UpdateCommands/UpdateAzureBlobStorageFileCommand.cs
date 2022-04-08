using Audacia.Azure.BlobStorage.Commands.AddCommands;

namespace Audacia.Azure.BlobStorage.Commands.UpdateCommands
{
    public class UpdateAzureBlobStorageFileCommand : BaseUpdateBlobStorageCommand
    {
        /// <summary>
        /// The full file path to the location of the file which you want to upload.
        /// </summary>
        public string FilePath { get; }

        public UpdateAzureBlobStorageFileCommand(string containerName, string blobName, string filePath,
            bool doesContainerExist = true) : base(containerName, blobName, doesContainerExist)
        {
            FilePath = filePath;
        }
    }
}