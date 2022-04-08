using System;
using Audacia.Azure.BlobStorage.Models;

namespace Audacia.Azure.BlobStorage.Exceptions.BlobDataExceptions
{
    public class BlobDataCannotBeEmptyException : Exception
    {
        private const string TemplateExceptionMessage = "Cannot add Blob: {0} which is of type {1} be empty";

        public BlobDataCannotBeEmptyException(string blobName, BlobDataType blobDataType) : base(
            string.Format(TemplateExceptionMessage, blobName, blobDataType.ToString()))
        {
        }
    }
}