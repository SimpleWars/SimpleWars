namespace SimpleWars.DBContexts
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    using SimpleWars.Migrations;
    using SimpleWars.Models.Economy;
    using SimpleWars.Models.Entities.DynamicEntities;
    using SimpleWars.Models.Entities.StaticEntities;
    using SimpleWars.Models.Users;

    public class GameContext : DbContext
    {
        public GameContext()
            : base("name=GameDbConnection")
        {
            Database.SetInitializer<GameContext>(new MigrateDatabaseToLatestVersion<GameContext, Configuration>());
        }

        public virtual DbSet<ResourceProvider> ResourceProviders { get; set; }

        public virtual DbSet<Unit> Units { get; set; }

        public virtual DbSet<Resource> Resources { get; set; }

        public virtual DbSet<ResourceSet> ResourceSets { get; set; }

        public virtual DbSet<Player> Players { get; set; }

    }
}