namespace Audacia.Azure.ReturnOptions.ClassOption
{
    public class ReturnClassOption<T> : IQueueReturnOption<T>
    {
        public T Result { get; set; }

        public T Parse(string jsonString)
        {
            throw new System.NotImplementedException();
        }
    }
}