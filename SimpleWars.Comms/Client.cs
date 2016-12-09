namespace SimpleWars.Comms
{
    using System.Net;

    public static class Client
    {
        public static readonly AsynchronousSocketClient Socket = new AsynchronousSocketClient();
    }
}