﻿namespace SimpleWars.ModelDTOs
{
    using ProtoBuf;

    [ProtoContract]
    public class AuthDTO
    {
        [ProtoMember(1)]
        public string Username { get; set; }

        [ProtoMember(2)]
        public string PasswordHash { get; set; }

        protected AuthDTO()
        {
        }

        public AuthDTO(string username, string passwordHash)
        {
            this.Username = username;
            this.PasswordHash = passwordHash;
        }
    }
}