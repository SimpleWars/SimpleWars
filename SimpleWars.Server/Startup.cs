namespace SimpleWars.Server
{
    using SimpleWars.Server.Listeners;

    class Startup
    {
        static void Main()
        {
            AsynchronousSocketListener.StartListening();
        }
    }
}
