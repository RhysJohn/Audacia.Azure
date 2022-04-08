using Audacia.Azure.BlobStorage.Commands.AddCommands;

namespace Audacia.Azure.BlobStorage.Commands.UpdateCommands
{
    public class UpdateAzureBlobStorageBytesCommand : BaseUpdateBlobStorageCommand
    {
        /// <summary>
        /// Array fo bytes which contains the data of the information which you want to upload as a blob.
        /// </summary>
        public byte[] BlobData { get; }

        public UpdateAzureBlobStorageBytesCommand(string containerName, string blobName, byte[] blobData,
            bool doesContainerExist = true) : base(containerName, blobName, doesContainerExist)
        {
            BlobData = blobData;
        }
    }
}