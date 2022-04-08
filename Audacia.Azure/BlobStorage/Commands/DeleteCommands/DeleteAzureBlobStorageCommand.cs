namespace Audacia.Azure.BlobStorage.Commands.DeleteCommands
{
    public class DeleteAzureBlobStorageCommand : BaseBlobCommand
    {
        public DeleteAzureBlobStorageCommand(string containerName, string blobName) : base(containerName, blobName)
        {
        }
    }
}