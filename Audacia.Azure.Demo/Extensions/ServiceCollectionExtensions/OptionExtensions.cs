using Audacia.Azure.BlobStorage.Config;
using Audacia.Azure.StorageQueue.Config;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Audacia.Azure.Demo.Extensions.ServiceCollectionExtensions
{
    public static class OptionExtensions
    {
        /// <summary>
        /// Used for adding all the config options to the IoC.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddConfigOptions(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            return serviceCollection
                .Configure<BlobStorageOption>(configuration.GetSection(BlobStorageOption.OptionConfigLocation))
                .Configure<QueueStorageOption>(configuration.GetSection(QueueStorageOption.OptionConfigLocation));
        }

        /// <summary>
        /// Adds the client to the IoC.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddAzureClients(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            return serviceCollection.AddSingleton(serviceProvider =>
                new BlobServiceClient(configuration.GetConnectionString("AzureBlobStorageConnectionString")));
        }
    }
}