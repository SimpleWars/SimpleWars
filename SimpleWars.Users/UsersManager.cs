namespace SimpleWars.Users
{
    using System;
    using System.Linq;
    using System.Data.Entity;
    using System.Security.Cryptography;
    using System.Text;

    using Microsoft.Xna.Framework;

    using SimpleWars.Data.Contexts;
    using SimpleWars.Models.Users;
    using SimpleWars.Models.Users.Interfaces;

    public static class UsersManager
    {
        private static readonly Random RandomSeeder = new Random();

        public static IPlayer CurrentPlayer { get; private set; }

        public static string LoginUser(string username, string password, GameContext context)
        {
            string hashedPassword = HashPassword(password);
            var player =
                context.Players.FirstOrDefault(p => p.Username == username && p.HashedPassword == hashedPassword);

            hashedPassword = string.Empty;

            if (player == null)
            {
                return "Invalid credentials";
            }

            CurrentPlayer = player;

            return "Successful login";
        }

        public static string RegisterUser(string username, string password, GameContext context)
        {
            var player = context.Players.FirstOrDefault(p => p.Username == username);

            if (player != null)
            {
                return "Username already taken";
            }

            int homeWorldSeed = RandomSeeder.Next(0, 1000000000);
            float homeX = (float)RandomSeeder.NextDouble() * 1000;
            float homeY = (float)RandomSeeder.NextDouble() * 500;

            string hashedPassword = HashPassword(password);
            var registeredPlayer = new Player(username, hashedPassword, homeWorldSeed, new Vector2(homeX, homeY));
            context.Players.Add(registeredPlayer);
            context.SaveChanges();
            CurrentPlayer =
                context.Players.FirstOrDefault(p => p.Username == username && p.HashedPassword == hashedPassword);

            hashedPassword = string.Empty;

            return "Successful registration";
        }

        public static string LogoutCurrentUser(GameContext context)
        {
            if (CurrentPlayer == null)
            {
                return "You must be logged in";
            }

            context.SaveChanges();
            CurrentPlayer = null;

            return "Successful logout";
        }

        private static string HashPassword(string password)
        {
            SHA256Managed crypt = new SHA256Managed();
            StringBuilder hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(password), 0, Encoding.UTF8.GetByteCount(password));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }

            return hash.ToString();
        }
    }
}