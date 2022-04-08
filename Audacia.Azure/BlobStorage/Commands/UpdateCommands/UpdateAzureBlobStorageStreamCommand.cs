using System.IO;
using Audacia.Azure.BlobStorage.Commands.AddCommands;

namespace Audacia.Azure.BlobStorage.Commands.UpdateCommands
{
    public class UpdateAzureBlobStorageStreamCommand : BaseUpdateBlobStorageCommand
    {
        /// <summary>
        /// Stream of the blob which you want to upload to storage.
        /// </summary>
        public Stream BlobData { get; }

        public UpdateAzureBlobStorageStreamCommand(string containerName, string blobName, Stream blobData,
            bool doesContainerExist = true) : base(containerName, blobName, doesContainerExist)
        {
            BlobData = blobData;
        }
    }
}