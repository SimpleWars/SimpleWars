namespace SimpleWars.User
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.Xna.Framework;

    using SimpleWars.GameData.Entities;
    using SimpleWars.GameData.Resources;

    public class Player
    {
        private int homeSeed;

        /// <summary>
        /// Empty constructor for stupid ORM
        /// </summary>
        private Player()
        {
        }

        public Player(
            string name,
            string hashedPassword,
            int homeSeed,
            Vector2 worldMapPos,
            ICollection<Entity> entities,
            ResourceSet resourceSet)
        {
            this.Username = name;
            this.HashedPassword = hashedPassword;
            this.HomeSeed = homeSeed;
            this.Entities = entities;
            this.ResourceSet = resourceSet;
            this.ResourceSetId = this.ResourceSet.Id;
            this.WorldMapPos = worldMapPos;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        [Required]
        public string Username { get; private set; }

        [Required]
        public string HashedPassword { get; private set; }

        public int HomeSeed
        {
            get
            {
                return this.homeSeed;
            }
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Home seed cannot be negative");
                }

                this.homeSeed = value;
            }
        }

        [ForeignKey("ResourceSet")]
        public int ResourceSetId { get; private set; }

        public ResourceSet ResourceSet { get; private set; }

        public float WorldX
        {
            get
            {
                return this.WorldMapPos.X;
            }

            private set
            {
                this.WorldMapPos = new Vector2(value, this.WorldMapPos.Y);
            }
        }

        public float WorldY
        {
            get
            {
                return this.WorldMapPos.Y;
            }

            private set
            {
                this.WorldMapPos = new Vector2(this.WorldMapPos.X, value);
            }
        }

        public ICollection<Entity> Entities { get; private set; }

        [NotMapped]
        public Vector2 WorldMapPos { get; private set; }
    }
}