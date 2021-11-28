namespace Audacia.Azure.ReturnOptions.ImageOption
{
    public class ReturnBytesOption : IBlobReturnOption<byte[]>
    {
        private string _blobName;

        /// <summary>
        /// Name of the blob received from the storage account.
        /// </summary>
        public string BlobName => _blobName;

        private byte[] _result;

        /// <summary>
        /// The returning value from blob storage containing the base 64 string of the image.
        /// </summary>
        public byte[] Result => _result;

        /// <summary>
        /// Returns an array of bytes for the data of the Image from Blob storage.
        /// </summary>
        /// <param name="blobName"></param>
        /// <param name="bytes">The bytes of the image returned from blob storage.</param>
        /// <returns>An array bytes which contains the data for the image.</returns>
        public byte[] Parse(string blobName, byte[] bytes, string blobClientUrl = null)
        {
            _blobName = blobName;
            _result = bytes;

            return bytes;
        }
    }
}