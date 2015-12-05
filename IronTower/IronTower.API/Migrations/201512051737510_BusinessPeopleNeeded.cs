namespace IronTower.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BusinessPeopleNeeded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Businesses", "RateOfPopulation", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Businesses", "RateOfPopulation");
        }
    }
}
