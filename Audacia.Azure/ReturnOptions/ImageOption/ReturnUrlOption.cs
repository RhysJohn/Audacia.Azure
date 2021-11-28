using System;

namespace Audacia.Azure.ReturnOptions.ImageOption
{
    public class ReturnUrlOption : IBlobReturnOption<string>
    {
        private string _blobName;

        /// <summary>
        /// Name of the blob received from the storage account.
        /// </summary>
        public string BlobName => _blobName;

        private string _result;

        /// <summary>
        /// The returning value from blob storage containing the guid which it was saved within the storage account.
        /// </summary>
        public string Result => _result;

        public string Parse(string blobName, byte[] bytes, string blobClientUrl = null)
        {
            if (blobClientUrl == null)
            {
                throw new Exception("Need to provide the blob service client url for return the url option");
            }
            _blobName = blobName;
            _result = $"{blobClientUrl}/{blobName}";

            return Result;
        }
    }
}