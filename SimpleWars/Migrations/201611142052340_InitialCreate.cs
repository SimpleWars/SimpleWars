namespace SimpleWars.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Entities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PosX = c.Single(nullable: false),
                        PosY = c.Single(nullable: false),
                        PosZ = c.Single(nullable: false),
                        RotX = c.Single(nullable: false),
                        RotY = c.Single(nullable: false),
                        RotZ = c.Single(nullable: false),
                        Scale = c.Single(nullable: false),
                        Weight = c.Single(nullable: false),
                        OwnerId = c.Int(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Players", t => t.OwnerId, cascadeDelete: true)
                .Index(t => t.OwnerId);
            
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false),
                        HashedPassword = c.String(nullable: false),
                        HomeSeed = c.Int(nullable: false),
                        ResourceSetId = c.Int(nullable: false),
                        WorldX = c.Single(nullable: false),
                        WorldY = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ResourceSets", t => t.ResourceSetId, cascadeDelete: true)
                .Index(t => t.ResourceSetId);
            
            CreateTable(
                "dbo.ResourceSets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Food_Id = c.Int(),
                        Gold_Id = c.Int(),
                        Metal_Id = c.Int(),
                        Population_Id = c.Int(),
                        Rock_Id = c.Int(),
                        Wood_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Resources", t => t.Food_Id)
                .ForeignKey("dbo.Resources", t => t.Gold_Id)
                .ForeignKey("dbo.Resources", t => t.Metal_Id)
                .ForeignKey("dbo.Resources", t => t.Population_Id)
                .ForeignKey("dbo.Resources", t => t.Rock_Id)
                .ForeignKey("dbo.Resources", t => t.Wood_Id)
                .Index(t => t.Food_Id)
                .Index(t => t.Gold_Id)
                .Index(t => t.Metal_Id)
                .Index(t => t.Population_Id)
                .Index(t => t.Rock_Id)
                .Index(t => t.Wood_Id);
            
            CreateTable(
                "dbo.Resources",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Entities", "OwnerId", "dbo.Players");
            DropForeignKey("dbo.Players", "ResourceSetId", "dbo.ResourceSets");
            DropForeignKey("dbo.ResourceSets", "Wood_Id", "dbo.Resources");
            DropForeignKey("dbo.ResourceSets", "Rock_Id", "dbo.Resources");
            DropForeignKey("dbo.ResourceSets", "Population_Id", "dbo.Resources");
            DropForeignKey("dbo.ResourceSets", "Metal_Id", "dbo.Resources");
            DropForeignKey("dbo.ResourceSets", "Gold_Id", "dbo.Resources");
            DropForeignKey("dbo.ResourceSets", "Food_Id", "dbo.Resources");
            DropIndex("dbo.ResourceSets", new[] { "Wood_Id" });
            DropIndex("dbo.ResourceSets", new[] { "Rock_Id" });
            DropIndex("dbo.ResourceSets", new[] { "Population_Id" });
            DropIndex("dbo.ResourceSets", new[] { "Metal_Id" });
            DropIndex("dbo.ResourceSets", new[] { "Gold_Id" });
            DropIndex("dbo.ResourceSets", new[] { "Food_Id" });
            DropIndex("dbo.Players", new[] { "ResourceSetId" });
            DropIndex("dbo.Entities", new[] { "OwnerId" });
            DropTable("dbo.Resources");
            DropTable("dbo.ResourceSets");
            DropTable("dbo.Players");
            DropTable("dbo.Entities");
        }
    }
}
