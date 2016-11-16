namespace SimpleWars.Data.Migrations
{
    using System.Data.Entity.Migrations;

    using Microsoft.Xna.Framework;

    using SimpleWars.Data.Contexts;
    using SimpleWars.Models.Users;

    internal sealed class Configuration : DbMigrationsConfiguration<GameContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
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
