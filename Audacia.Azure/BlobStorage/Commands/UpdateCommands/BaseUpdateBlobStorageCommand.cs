namespace Audacia.Azure.BlobStorage.Commands.UpdateCommands
{
    public abstract class BaseUpdateBlobStorageCommand : BaseBlobCommand
    {
        /// <summary>
        /// Whether or not the container already exist.
        /// </summary>
        public bool DoesContainerExist { get; }

        public BaseUpdateBlobStorageCommand(string containerName, string blobName, bool doesContainerExist = true) : base(
            containerName, blobName)
        {
            DoesContainerExist = doesContainerExist;
        }
    }
}