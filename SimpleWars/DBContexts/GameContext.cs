namespace SimpleWars.DBContexts
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    using SimpleWars.GameData.Entities;
    using SimpleWars.GameData.Resources;
    using SimpleWars.User;

    public class GameContext : DbContext
    {
        public GameContext()
            : base("name=GameDbConnection")
        {
            Database.SetInitializer<GameContext>(new DropCreateDatabaseIfModelChanges<GameContext>());
        }

        public virtual DbSet<Entity> Entities { get; set; }

        public virtual DbSet<Player> Players { get; set; }

        public virtual DbSet<Resource> Resources { get; set; }
    }
}