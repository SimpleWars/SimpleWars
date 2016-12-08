namespace SimpleWars.Comms
{
    using System.Collections.Concurrent;
    using System.Threading;

    using SimpleWars.ModelDTOs;

    public class MessageQueue
    {
        private readonly BlockingCollection<Message> queue;

        public MessageQueue()
        {
            this.queue = new BlockingCollection<Message>();
        }

        public void Enqueue(Message message)
        {
            this.queue.Add(message);
        }

        public Message Dequeue()
        {
            Message msg;
            bool took = this.queue.TryTake(out msg);
            return took ? msg : null;
        }

        public Message TryDequeue()
        {
            Message msg;
            bool took = this.queue.TryTake(out msg, Timeout.Infinite);
            return took ? msg : null;
        }
    }
}