namespace IronTower.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IronTowerGames",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Player = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        Update = c.DateTime(nullable: false),
                        TotalMoney = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Floors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        Update = c.DateTime(nullable: false),
                        Business_Id = c.Int(),
                        Game_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Businesses", t => t.Business_Id)
                .ForeignKey("dbo.IronTowerGames", t => t.Game_Id)
                .Index(t => t.Business_Id)
                .Index(t => t.Game_Id);
            
            CreateTable(
                "dbo.Businesses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Cost = c.Double(nullable: false),
                        EarningsPerMinute = c.Double(nullable: false),
                        NumberOfPeopleNeeded = c.Int(nullable: false),
                        Category_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.Category_Id)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Floors", "Game_Id", "dbo.IronTowerGames");
            DropForeignKey("dbo.Businesses", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.Floors", "Business_Id", "dbo.Businesses");
            DropIndex("dbo.Businesses", new[] { "Category_Id" });
            DropIndex("dbo.Floors", new[] { "Game_Id" });
            DropIndex("dbo.Floors", new[] { "Business_Id" });
            DropTable("dbo.Categories");
            DropTable("dbo.Businesses");
            DropTable("dbo.Floors");
            DropTable("dbo.IronTowerGames");
        }
    }
}
