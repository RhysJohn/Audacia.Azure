using System;

namespace Audacia.Azure.StorageQueue.Exceptions
{
    public class QueueDoesNotExistException : Exception
    {
        private const string TemplateExceptionMessage = "There is no queue with the name: {0}";

        public QueueDoesNotExistException(string queueName) : base(string.Format(TemplateExceptionMessage, queueName))
        {
        }
    }
}