using Audacia.Azure.BlobStorage.Commands.AddCommands;

namespace Audacia.Azure.BlobStorage.Commands.UpdateCommands
{
    public class UpdateAzureBlobStorageBase64Command : BaseUpdateBlobStorageCommand
    {
        /// <summary>
        /// Base 64 string containing the information of the blob you want to upload.
        /// </summary>
        public string BlobData { get; }

        public UpdateAzureBlobStorageBase64Command(string containerName,
            string blobName,
            string blobData,
            bool doesContainerExist = true) : base(containerName, blobName, doesContainerExist)
        {
            BlobData = blobData;
        }
    }
}