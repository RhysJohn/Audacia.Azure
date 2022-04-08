namespace Audacia.Azure.BlobStorage.Commands.AddCommands
{
    public class AddAzureBlobStorageBytesCommand : BaseAddBlobStorageCommand
    {
        /// <summary>
        /// Array fo bytes which contains the data of the information which you want to upload as a blob.
        /// </summary>
        public byte[] BlobData { get; }

        public AddAzureBlobStorageBytesCommand(string containerName, string blobName, byte[] blobData,
            bool doesContainerExist = true) : base(containerName, blobName, doesContainerExist)
        {
            BlobData = blobData;
        }
    }
}