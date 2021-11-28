using System.Collections.Generic;
using System.Linq;
using Azure.Storage.Queues.Models;

namespace Audacia.Azure.StorageQueue.Extensions
{
    public static class PeekedMessages
    {
        /// <summary>
        /// Returns all the <see cref="PeekedMessage"/> with the <see cref="PeekedMessage.MessageId"/> of <paramref name="messageId"/>.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public static IEnumerable<QueueMessage> WithMessageId(this IEnumerable<QueueMessage> source, string messageId)
        {
            return source.Where(peekedMessage => peekedMessage.MessageId == messageId);
        }
    }
}