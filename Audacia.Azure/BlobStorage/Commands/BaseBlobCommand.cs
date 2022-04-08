namespace Audacia.Azure.BlobStorage.Commands
{
    public abstract class BaseBlobCommand
    {
        /// <summary>
        /// The name of the container where you want the blob to be added.
        /// </summary>
        public string ContainerName { get; }

        /// <summary>
        /// The name of the blob which is going to be added to the storage account.
        /// </summary>
        public string BlobName { get; }

        public BaseBlobCommand(string containerName, string blobName)
        {
            ContainerName = containerName;
            BlobName = blobName;
        }
    }
}