namespace SimpleWars.User
{
    using SimpleWars.User.Interfaces;

    public static class PlayerManager
    {
        public static IPlayer CurrentPlayer { get; set; }
    }
}