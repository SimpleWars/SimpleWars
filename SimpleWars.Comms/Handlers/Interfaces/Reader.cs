namespace Server.CommHandlers.Interfaces
{
    public interface Reader
    {
        void ReadSingleMessage();

        void ReadMessagesContinuously();
    }
}