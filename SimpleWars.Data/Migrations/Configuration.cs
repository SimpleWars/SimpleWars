namespace SimpleWars.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Microsoft.Xna.Framework;

    using SimpleWars.DBContexts;
    using SimpleWars.Models.Entities.StaticEntities.ResourceProviders;
    using SimpleWars.Models.Users;

    internal sealed class Configuration : DbMigrationsConfiguration<GameContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(GameContext context)
        {
            // Uncomment to debug the seed
            // if (System.Diagnostics.Debugger.IsAttached == false)
            //    System.Diagnostics.Debugger.Launch();

            // This method will be called after migrating to the latest version.
            var player = new Player("Gosho", "123", 190231, Vector2.Zero);

            context.Players.AddOrUpdate(p => p.Username, player);
        }
    }
}
