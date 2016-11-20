namespace SimpleWars.Server.ListenerUtils
{
    using System.Net.Sockets;
    using System.Text;

    public class StateObject
    {
        // Size of receive buffer.
        public const int BufferSize = 1024;

        // Client  socket.
        public Socket workSocket = null;

        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];

        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }
}