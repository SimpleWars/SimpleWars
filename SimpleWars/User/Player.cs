namespace SimpleWars.User
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.Xna.Framework;

    using SimpleWars.GameData.EconomyData;
    using SimpleWars.GameData.Entities;
    using SimpleWars.GameData.Entities.DynamicEntities;
    using SimpleWars.GameData.Entities.Interfaces;
    using SimpleWars.GameData.Entities.StaticEntities;
    using SimpleWars.User.Interfaces;

    public class Player : IPlayer
    {
        private int homeSeed;

        /// <summary>
        /// Empty constructor for stupid ORM
        /// </summary>
        protected Player()
        {
            this.ResourceProviders = new HashSet<ResourceProvider>();
            this.Units = new HashSet<Unit>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="hashedPassword">
        /// The hashed password.
        /// </param>
        /// <param name="homeSeed">
        /// The home seed.
        /// </param>
        /// <param name="worldMapPos">
        /// The world map pos.
        /// </param>
        public Player(
            string name,
            string hashedPassword,
            int homeSeed,
            Vector2 worldMapPos)
        {
            this.Username = name;
            this.HashedPassword = hashedPassword;

            this.HomeSeed = homeSeed;         
            this.WorldMapPos = worldMapPos;

            this.ResourceProviders = new HashSet<ResourceProvider>();
            this.Units = new HashSet<Unit>();
            this.Resources = new ResourceSet(true);
        }

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

        [Required]
        public int ResourcesId { get; private set; }

        [Required, ForeignKey("ResourcesId")]
        public virtual ResourceSet Resources { get; private set; }

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

        public virtual ICollection<ResourceProvider> ResourceProviders { get; private set; }

        public virtual ICollection<Unit> Units { get; private set; }

        [NotMapped]
        public Vector2 WorldMapPos { get; private set; }
    }
}