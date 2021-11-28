using System;

namespace Audacia.Azure.BlobStorage.Exceptions
{
    public class ContainerAlreadyExistsException : Exception
    {
        private const string TemplateExceptionMessage =
            "There is already a container on this storage account with the name: {0}";

        public ContainerAlreadyExistsException(string containerName) : base(
            string.Format(TemplateExceptionMessage, containerName))
        {
        }
    }
}