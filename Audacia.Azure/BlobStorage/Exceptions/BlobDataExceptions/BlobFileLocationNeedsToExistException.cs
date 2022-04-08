using System;
using Audacia.Azure.BlobStorage.Models;

namespace Audacia.Azure.BlobStorage.Exceptions.BlobDataExceptions
{
    public class BlobFileLocationNeedsToExistException : Exception
    {
        private const string TemplateExceptionMessage =
            "Cannot add Blob: {0} which is of type {1} be null as doesnt exist on file system {2}";

        public BlobFileLocationNeedsToExistException(string blobName, BlobDataType blobDataType, string fileLocation) :
            base(
                string.Format(TemplateExceptionMessage, blobName, blobDataType.ToString(), fileLocation))
        {
        }
    }
}