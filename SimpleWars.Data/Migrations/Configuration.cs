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
        }
    }
}
