namespace Server.CommHandlers.Interfaces
{
    using SimpleWars.ModelDTOs;

    public interface Writer
    {
        void Send(Message message);
    }
}