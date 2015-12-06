namespace IronTower.API.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<IronTower.API.Models.IronTowerDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(IronTower.API.Models.IronTowerDBContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Businesses.AddOrUpdate(x => x.Category,
                 new Models.Business { Id = 1, Cost = 2000, EarningsPerMinute = 0, Category = "Residential", NumberOfPeopleNeeded = 5, RateOfPopulation = 1},
                 new Models.Business { Id = 2, Cost = 2000, EarningsPerMinute = 1000, Category = "Music Studio", NumberOfPeopleNeeded = 3, RateOfPopulation = 0},
                 new Models.Business { Id = 3, Cost = 4000, EarningsPerMinute = 2000, Category = "Coffee Shop", NumberOfPeopleNeeded = 3, RateOfPopulation = 0}
                );

        }
    }
}
