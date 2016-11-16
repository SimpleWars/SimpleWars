namespace SimpleWars.DBContexts
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    using SimpleWars.GameData.EconomyData;
    using SimpleWars.GameData.Entities;
    using SimpleWars.GameData.Entities.DynamicEntities;
    using SimpleWars.GameData.Entities.Interfaces;
    using SimpleWars.GameData.Entities.StaticEntities;
    using SimpleWars.Migrations;
    using SimpleWars.User;
    using SimpleWars.User.Interfaces;

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