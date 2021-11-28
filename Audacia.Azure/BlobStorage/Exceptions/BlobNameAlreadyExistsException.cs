using System;

namespace Audacia.Azure.BlobStorage.Exceptions
{
    public class BlobNameAlreadyExistsException : Exception
    {
        private const string TemplateExceptionMessage =
            "Cannot add Blob: {0} within Container: {1} as there is an existing blob with that name therefore preventing overwriting of any data";

        public BlobNameAlreadyExistsException(string blobName, string containerName) : base(
            string.Format(TemplateExceptionMessage, blobName, containerName))
        {
        }
    }
}