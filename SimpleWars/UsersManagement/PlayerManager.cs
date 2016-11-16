namespace SimpleWars.UsersManagement
{
    using SimpleWars.Models.Users.Interfaces;

    public static class PlayerManager
    {
        public static IPlayer CurrentPlayer { get; set; }
    }
}