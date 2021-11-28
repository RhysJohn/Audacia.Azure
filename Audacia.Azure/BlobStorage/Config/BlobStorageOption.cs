namespace Audacia.Azure.BlobStorage.Config
{
    /// <summary>
    /// Config class used for added the relevant config options needed to connect Azure blob storage account.
    /// </summary>
    public class BlobStorageOption
    {
        public const string OptionConfigLocation = "Azure:BlobStorageConfig";
        
        public string AccountName { get; set; }

        public string AccountKey { get; set; }
    }
}