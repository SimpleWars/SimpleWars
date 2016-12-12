namespace SimpleWars.Models.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Economy;
    using Entities;
    using Entities.DynamicEntities;
    using Entities.StaticEntities;
    using Interfaces;

    public class Player : IPlayer
    {
        private int homeSeed;

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
            Guid id,
            string name,
            int homeSeed,
            ResourceSet resourceSet)
        {
            this.Id = id;
            this.Username = name;
            this.HomeSeed = homeSeed;         

            this.ResourceProviders = new HashSet<ResourceProvider>();
            this.Units = new HashSet<Unit>();
            this.ResourceSet = resourceSet;
        }

        public Guid Id { get; private set; }

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