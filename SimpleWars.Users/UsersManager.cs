namespace SimpleWars.Users
{
    using System.Security.Cryptography;
    using System.Text;
    using Comms;
    using ModelDTOs;
    using ModelDTOs.Enums;
    using Models.Users;

    public static class UsersManager
    {
        public static Player CurrentPlayer { get; set; }

        public static void LoginUser(string username, string password)
        {
            Client.Socket.Writer.Send(Message.Create(Service.Login, new AuthDTO(username, HashPassword(password))));;
        }

        public static void RegisterUser(string username, string password)
        {
            Client.Socket.Writer.Send(Message.Create(Service.Registration, new AuthDTO(username, HashPassword(password))));
        }

        public static void LogoutCurrentUser()
        {
            Client.Socket.Writer.Send(new Message<byte>(Service.Logout, 1));
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