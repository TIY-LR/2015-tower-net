namespace IronTower.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFloorNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Floors", "FloorNumber", c => c.Int(nullable: false));
            DropColumn("dbo.Floors", "FloorRevenue");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Floors", "FloorRevenue", c => c.Int(nullable: false));
            DropColumn("dbo.Floors", "FloorNumber");
        }
    }
}
