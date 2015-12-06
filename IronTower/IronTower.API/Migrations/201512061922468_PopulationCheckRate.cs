namespace IronTower.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class PopulationCheckRate : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.IronTowerGames", "Update", "PopulationUpdate");
            AddColumn("dbo.IronTowerGames", "PopulationCheckRate", c => c.Int(nullable: false, defaultValue: 60));
        }

        public override void Down()
        {
            AddColumn("dbo.IronTowerGames", "Update", c => c.DateTime(nullable: false));
            DropColumn("dbo.IronTowerGames", "PopulationCheckRate");
        }
    }
}
