namespace Audacia.Azure.BlobStorage.Commands.DeleteCommands
{
    public class DeleteAzureBlobStorageCommand : BaseBlobCommand
    {
        /// <summary>
        /// Command uses to delete a blob.
        /// </summary>
        /// <param name="containerName"></param>
        /// <param name="blobName"></param>
        public DeleteAzureBlobStorageCommand(string containerName, string blobName) : base(containerName, blobName)
        {
        }
    }
}