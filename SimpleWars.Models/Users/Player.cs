namespace SimpleWars.Models.Users
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    using Microsoft.Xna.Framework;

    using SimpleWars.Models.Economy;
    using SimpleWars.Models.Entities;
    using SimpleWars.Models.Entities.DynamicEntities;
    using SimpleWars.Models.Entities.Interfaces;
    using SimpleWars.Models.Entities.StaticEntities;
    using SimpleWars.Models.Users.Interfaces;

    public class Player : IPlayer
    {
        private int homeSeed;

        /// <summary>
        /// Empty constructor for EF
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
            this.ResourceSet = new ResourceSet(true);
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
        public virtual ResourceSet ResourceSet { get; private set; }

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

        
        public IEnumerable<IEntity> AllEntities => this.ResourceProviders.Concat<IEntity>(this.Units);

        
        public Vector2 WorldMapPos { get; private set; }
    }
}