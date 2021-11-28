namespace Audacia.Azure.ReturnOptions
{
    public interface IBlobReturnOption<T>
    {
        string BlobName { get; }

        T Result { get; }

        T Parse(string blobName, byte[] bytes, string blobClientUrl = null);
    }
}