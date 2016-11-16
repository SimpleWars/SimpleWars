namespace SimpleWars.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Microsoft.Xna.Framework;

    using SimpleWars.DBContexts;
    using SimpleWars.GameData.Entities;
    using SimpleWars.GameData.Entities.StaticEntities.Environment;
    using SimpleWars.User;

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
            if (context.Players.Find(1) != null)
            {
                return;
            }

            var player = new Player("Gosho", "123", 190231, Vector2.Zero);
            var random = new Random();
            var numberOfTrees = random.Next(300, 400);

            for (int i = 0; i < numberOfTrees; i++)
            {
                var x = random.Next(-200, 200);
                var z = random.Next(-200, 200);
                var weight = random.Next(5, 10);
                var y = 100;

                var tree = new Tree(new Vector3(x, y, z), Vector3.Zero, weight, 1);
                player.ResourceProviders.Add(tree);
            }

            context.Players.AddOrUpdate(p => p.Username, player);
        }
    }
}
