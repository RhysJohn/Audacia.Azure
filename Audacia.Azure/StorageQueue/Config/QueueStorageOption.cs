namespace Audacia.Azure.StorageQueue.Config
{
    /// <summary>
    /// Config class used for added the relevant config options needed to connect Azure queue storage account.
    /// </summary>
    public class QueueStorageOption
    {
        public const string OptionConfigLocation = "Azure:QueueStorageConfig";
        
        public string AccountName { get; set; }

        public string AccountKey { get; set; }
    }
}