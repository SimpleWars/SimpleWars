namespace SimpleWars.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Microsoft.Xna.Framework;

    using SimpleWars.GameData.Entities;
    using SimpleWars.GameData.Resources;
    using SimpleWars.User;

    internal sealed class Configuration : DbMigrationsConfiguration<SimpleWars.DBContexts.GameContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(SimpleWars.DBContexts.GameContext context)
        {
            // This method will be called after migrating to the latest version.
            context.Players.Add(new Player("Gosho", "123", 190231, Vector2.Zero, new List<Entity>(), new ResourceSet()));
        }
    }
}
