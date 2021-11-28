using System;

namespace Audacia.Azure.BlobStorage.Exceptions
{
    public class BlobStorageConfigurationException : Exception
    {
        private const string OptionsNotConfiguredExceptionMessage =
            "Need to add a value of IOptions<BlobStorageConfig> to the DI";

        private const string MissingConfigExceptionMessage =
            "Cannot connect to an Azure Blob storage with an null/empty {0}";

        private const string BlobClientNotConfiguredExceptionMessage =
            "Need to add BlobServiceClient to the DI";

        public BlobStorageConfigurationException(string exceptionMessage) : base(exceptionMessage)
        {
        }

        public static BlobStorageConfigurationException OptionsNotConfigured()
        {
            return new BlobStorageConfigurationException(OptionsNotConfiguredExceptionMessage);
        }

        public static BlobStorageConfigurationException AccountNameNotConfigured()
        {
            return new BlobStorageConfigurationException(string.Format(MissingConfigExceptionMessage, "account name"));
        }

        public static BlobStorageConfigurationException AccountKeyNotConfigured()
        {
            return new BlobStorageConfigurationException(string.Format(MissingConfigExceptionMessage, "account key"));
        }

        public static BlobStorageConfigurationException BlobClientNotConfigured()
        {
            return new BlobStorageConfigurationException(BlobClientNotConfiguredExceptionMessage);
        }
    }
}