using System.Collections;
using Audacia.Azure.BlobStorage.Services;
using Audacia.Azure.BlobStorage.Services.Interfaces;
using Audacia.Azure.StorageQueue.Services;
using Audacia.Azure.StorageQueue.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Audacia.Azure.Demo.Extensions.ServiceCollectionExtensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddAzureBlobServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddScoped<IGetAzureBlobStorageService, GetAzureAzureBlobStorageService>()
                .AddScoped<IAddAzureBlobStorageService, AddAzureBlobStorageService>()
                .AddScoped<IUpdateAzureBlobStorageService, AzureUpdateAzureBlobStorageService>()
                .AddScoped<IDeleteAzureBlobStorageService, DeleteAzureBlobStorageService>();
        }

        public static IServiceCollection AddAzureQueueServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddScoped<IAddAzureQueueStorageService, AddAzureQueueStorageService>();
        }
    }
}