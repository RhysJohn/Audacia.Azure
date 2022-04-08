namespace Audacia.Azure.BlobStorage.Commands.AddCommands
{
    public abstract class BaseAddBlobStorageCommand : BaseBlobCommand
    {
        /// <summary>
        /// Whether or not the container already exist.
        /// </summary>
        public bool DoesContainerExist { get; }

        public BaseAddBlobStorageCommand(string containerName, string blobName, bool doesContainerExist = true) : base(
            containerName, blobName)
        {
            DoesContainerExist = doesContainerExist;
        }
    }
}