using System;

namespace Audacia.Azure.StorageQueue.Models
{
    /// <summary>
    /// Message object pulled from the queue.
    /// </summary>
    public class AzureQueueStorageMessage
    {
        public string MessageId { get; }

        public string PopReceipt { get; }

        public string Message { get; }

        public DateTime InsertedOn { get; set; }

        public DateTime Received { get; }

        public AzureQueueStorageMessage(string messageId,
            string popReceipt,
            string message,
            DateTime insertedOn,
            DateTime received)
        {
            MessageId = messageId;
            PopReceipt = popReceipt;
            Message = message;
            InsertedOn = insertedOn;
            Received = received;
        }
    }
}