namespace IronTower.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Businesses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Category = c.String(),
                        Cost = c.Int(nullable: false),
                        EarningsPerMinute = c.Int(nullable: false),
                        NumberOfPeopleNeeded = c.Int(nullable: false),
                        RateOfPopulation = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Floors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateCreated = c.DateTime(nullable: false),
                        Update = c.DateTime(nullable: false),
                        NumberOfEmployeesOrResidents = c.Int(nullable: false),
                        FloorRevenue = c.Int(nullable: false),
                        Business_Id = c.Int(),
                        Game_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Businesses", t => t.Business_Id)
                .ForeignKey("dbo.IronTowerGames", t => t.Game_Id)
                .Index(t => t.Business_Id)
                .Index(t => t.Game_Id);
            
            CreateTable(
                "dbo.IronTowerGames",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Player = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        Update = c.DateTime(nullable: false),
                        TotalMoney = c.Int(nullable: false),
                        TotalResidents = c.Int(nullable: false),
                        AvailableEmployees = c.Int(nullable: false),
                        Capacity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Floors", "Game_Id", "dbo.IronTowerGames");
            DropForeignKey("dbo.Floors", "Business_Id", "dbo.Businesses");
            DropIndex("dbo.Floors", new[] { "Game_Id" });
            DropIndex("dbo.Floors", new[] { "Business_Id" });
            DropTable("dbo.IronTowerGames");
            DropTable("dbo.Floors");
            DropTable("dbo.Businesses");
        }
    }
}
