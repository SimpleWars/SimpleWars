namespace SimpleWars.User
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.Xna.Framework;

    using SimpleWars.GameData.Entities;
    using SimpleWars.GameData.Resources;

    public class Player
    {
        private ICollection<Entity> entities;

        private ICollection<Resource> resources;

        public Player(int id, string name, uint homeSeed, ICollection<Entity> entities, ICollection<Resource> resources, Vector2 worldMapPosition)
        {
            this.Id = id;
            this.Name = name;
            this.HomeSeed = homeSeed;
            this.entities = entities;
            this.resources = resources;
            this.WorldMapPosition = worldMapPosition;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; }

        public string Name { get; }
        public uint HomeSeed { get; }

        public Vector2 WorldMapPosition { get; }
        
        [NotMapped]
        public IEnumerable<Entity> Entities => this.entities;

        [NotMapped]
        public IEnumerable<Resource> Resource => this.resources;
    }
}