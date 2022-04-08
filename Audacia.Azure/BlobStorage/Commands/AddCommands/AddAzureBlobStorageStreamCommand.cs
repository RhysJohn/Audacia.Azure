using System.IO;

namespace Audacia.Azure.BlobStorage.Commands.AddCommands
{
    public class AddAzureBlobStorageStreamCommand : BaseAddBlobStorageCommand
    {
        /// <summary>
        /// Stream of the blob which you want to upload to storage.
        /// </summary>
        public Stream BlobData { get; }

        public AddAzureBlobStorageStreamCommand(string containerName, string blobName, Stream blobData,
            bool doesContainerExist = true) : base(containerName, blobName, doesContainerExist)
        {
            BlobData = blobData;
        }
    }
}