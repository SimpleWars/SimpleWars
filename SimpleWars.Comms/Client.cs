namespace SimpleWars.Comms
{
    using System.Net;
    using System.Net.Sockets;

    public static class Client
    {
        public static readonly AsynchronousSocketClient Socket = new AsynchronousSocketClient();

        public static IPAddress GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }

            return IPAddress.Parse("127.0.0.1");
        }
    }
}