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
        /// <param name="entities">
        /// The entities.
        /// </param>
        /// <param name="resourceSet">
        /// The resource set.
        /// </param>
        /// <param name="id">
        /// The id. It is set-able only for testing purposes
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

            this.Entities = new HashSet<Entity>();
            this.ResourceSet = new ResourceSet();
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