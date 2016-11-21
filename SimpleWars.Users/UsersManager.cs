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
    using SimpleWars.Users.Enums;

    public static class UsersManager
    {
        private static readonly Random RandomSeeder = new Random();

        public static IPlayer CurrentPlayer { get; private set; }

        public static LoginState LoginUser(string username, string password, GameContext context)
        {
            try
            {
                string hashedPassword = HashPassword(password);
                var player =
                    context.Players.FirstOrDefault(p => p.Username == username && p.HashedPassword == hashedPassword);

                hashedPassword = string.Empty;

                if (player == null)
                {
                    return LoginState.Invalid;
                }

                CurrentPlayer = player;

                return LoginState.Successful;
            }
            catch (Exception)
            {
                return LoginState.Error;
            }
        }

        public static RegisterState RegisterUser(string username, string password, GameContext context)
        {
            try
            {
                var player = context.Players.FirstOrDefault(p => p.Username == username);

                if (player != null)
                {
                    return RegisterState.UsernameTaken;
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

                return RegisterState.Successful;
            }
            catch (Exception)
            {
                return RegisterState.Error;
            }
        }

        public static LogoutState LogoutCurrentUser(GameContext context)
        {
            try
            {
                if (CurrentPlayer == null)
                {
                    return LogoutState.NotLogged;
                }

                context.SaveChanges();
                CurrentPlayer = null;

                return LogoutState.Successful;
            }
            catch (Exception)
            {
                return LogoutState.Error;
            }
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