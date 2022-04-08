using System;
using Audacia.Azure.BlobStorage.Models;

namespace Audacia.Azure.BlobStorage.Exceptions.BlobDataExceptions
{
    public class BlobDataCannotBeInvalidBase64Exception : Exception
    {
        private const string TemplateExceptionMessage =
            "Cannot add Blob: {0} because the data value is invalid Base 64: {1}";

        public BlobDataCannotBeInvalidBase64Exception(string blobName, string blobData) : base(
            string.Format(TemplateExceptionMessage, blobName, blobData))
        {
        }
    }
}