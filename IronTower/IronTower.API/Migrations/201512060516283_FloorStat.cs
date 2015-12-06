namespace IronTower.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FloorStat : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Floors", "TotalMoneyMade", c => c.Double(nullable: false));
            AlterColumn("dbo.IronTowerGames", "TotalMoney", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.IronTowerGames", "TotalMoney", c => c.Int(nullable: false));
            DropColumn("dbo.Floors", "TotalMoneyMade");
        }
    }
}
