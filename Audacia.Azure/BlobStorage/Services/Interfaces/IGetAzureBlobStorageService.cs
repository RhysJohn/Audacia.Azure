using System.Collections.Generic;
using System.Threading.Tasks;
using Audacia.Azure.ReturnOptions;

namespace Audacia.Azure.BlobStorage.Services.Interfaces
{
    /// <summary>
    /// This should be added to the DI, along with registering it against the concrete type <see cref="GetAzureAzureBlobStorageService"/>.
    /// </summary>
    public interface IGetAzureBlobStorageService
    {
        /// <summary>
        /// Returns a blob with the name <paramref name="blobName"/> within the <paramref name="containerName"/>.
        /// </summary>
        /// <param name="containerName">The name of the container where the blob you want to return is stored in.</param>
        /// <param name="blobName">The name of the blob you are wanting to return.</param>
        /// <typeparam name="T">The type of data which you want the blob to be converted into. This must match one of the
        /// return options. Please look into the different return options to decide which is best suited for you.</typeparam>
        /// <typeparam name="TResponse">The return option which you want the blob to be returned in.</typeparam>
        /// <returns>A <see cref="IBlobReturnOption"/> which has been configured by the generic arguments.</returns>
        Task<T> GetAsync<T, TResponse>(string containerName, string blobName) where TResponse : IBlobReturnOption<T>, new ();

        /// <summary>
        /// Returns a collection of blob based on the collection of blob names <paramref name="blobNames"/> within the
        /// container within an Azure storage account.
        /// </summary>
        /// <param name="containerName">The name of the container where the blob you want to return is stored in.</param>
        /// <param name="blobNames">A collection of blob names you are wanting to return.</param>
        /// <typeparam name="T">The type of data which you want the blob to be converted into. This must match one of the
        /// return options. Please look into the different return options to decide which is best suited for you.</typeparam>
        /// <typeparam name="TResponse">The return option which you want the blob to be returned in.</typeparam>
        /// <returns>A <see cref="IBlobReturnOption"/> which has been configured by the generic arguments.</returns>
        Task<IDictionary<string, T>> GetSomeAsync<T, TResponse>(string containerName, IEnumerable<string> blobNames)
            where TResponse : IBlobReturnOption<T>, new ();

        /// <summary>
        /// Returns all the blob within a container within the storage account.
        /// </summary>
        /// <param name="containerName">The name of the container where the blobs you want to return are stored in.</param>
        /// <typeparam name="T">The type of data which you want the blob to be converted into. This must match one of the
        /// return options. Please look into the different return options to decide which is best suited for you.</typeparam>
        /// <typeparam name="TResponse">The return option which you want the blob to be returned in.</typeparam>
        /// <returns>A collection of <see cref="IBlobReturnOption"/> which has been configured by the generic arguments.</returns>
        Task<IDictionary<string, T>> GetAllAsync<T, TResponse>(string containerName)
            where TResponse : IBlobReturnOption<T>, new();
    }
}