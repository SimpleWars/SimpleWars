namespace SimpleWars.Comms
{
    using System.Net;
    using System.Net.Sockets;

    using Server.CommHandlers;
    using Server.CommHandlers.Interfaces;

    using SimpleWars.Utils;

    public class AsynchronousSocketClient
    {
        private const int BufferPoolSize = 50;

        // ~1 MB
        private const int MaxBufferSize = 1048576;

        public readonly Socket Socket;

        public readonly Reader Reader;

        public readonly Writer Writer;

        public readonly Buffers Buffers;

        public readonly MessageQueue MsgQueue;

        public AsynchronousSocketClient(IPEndPoint connectionEndPoint)
        {
            this.Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
           
            this.Socket.Connect(connectionEndPoint);
            this.Reader = new DefaultReader(this);
            this.Writer = new DefaultWriter(this);
            this.Buffers = new Buffers(BufferPoolSize, MaxBufferSize);
            this.MsgQueue = new MessageQueue();
        }

        public void Dispose()
        {
            this.Socket.Close();
            this.Socket.Dispose();
        }
    }
}