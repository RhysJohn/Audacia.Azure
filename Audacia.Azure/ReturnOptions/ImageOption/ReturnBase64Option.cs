using System;

namespace Audacia.Azure.ReturnOptions.ImageOption
{
    public class ReturnBase64Option : IBlobReturnOption<string>
    {
        private string _blobName;

        /// <summary>
        /// Name of the blob received from the storage account.
        /// </summary>
        public string BlobName => _blobName;

        /// <summary>
        /// The image type will default to <see cref="ImageType.Png"/>.
        /// </summary>
        public ImageType Type { get; set; } = ImageType.Png;

        private string _result;

        /// <summary>
        /// The returning value from blob storage containing the base 64 string of the image.
        /// </summary>
        public string Result => _result;

        /// <summary>
        /// Converts the bytes from blob storage to a base 64 string.
        /// </summary>
        /// <param name="blobName"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public string Parse(string blobName, byte[] bytes, string blobClientUrl = null)
        {
            _blobName = blobName;
            var base64String = Convert.ToBase64String(bytes);

            return $"data:image/{Type.ToString().ToLower()};base64,{base64String}";
        }
    }
}