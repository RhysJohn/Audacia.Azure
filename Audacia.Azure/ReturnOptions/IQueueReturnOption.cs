namespace Audacia.Azure.ReturnOptions
{
    public interface IQueueReturnOption<T>
    {
        T Result { get; set; }

        T Parse(string jsonString);
    }
}