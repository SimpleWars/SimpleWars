namespace SimpleWars.Models.Users
{
    using System;
    using System.Collections.Generic;
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
            int homeSeed)
        {
            this.Username = name;
            this.HomeSeed = homeSeed;         

            this.ResourceProviders = new HashSet<ResourceProvider>();
            this.Units = new HashSet<Unit>();
            this.ResourceSet = new ResourceSet(true);
        }

        public int Id { get; private set; }

        public string Username { get; private set; }

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

        public ResourceSet ResourceSet { get; private set; }

        public ICollection<ResourceProvider> ResourceProviders { get; private set; }

        public ICollection<Unit> Units { get; private set; }

        public ICollection<Entity> AllEntities { get; private set; }

        public void MapEntities()
        {
            this.AllEntities = 
                this.ResourceProviders.Concat<Entity>(this.Units).ToList();
        }
    }
}