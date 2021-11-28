using System;

namespace Audacia.Azure.BlobStorage.Exceptions
{
    public class ContainerDoesNotExistException : Exception
    {
        private const string TemplateExceptionMessage =
            "Container: {0} does not exist therefore unable to find the blob within the specified container";

        public ContainerDoesNotExistException(string containerName) : base(
            string.Format(TemplateExceptionMessage, containerName))
        {
        }
    }
}