namespace IronTower.API.Models
{
    using Migrations;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class IronTowerDBContext : DbContext
    {
        // Your context has been configured to use a 'IronTowerDBContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'IronTower.API.Models.IronTowerDBContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'IronTowerDBContext' 
        // connection string in the application configuration file.
        public IronTowerDBContext()
            : base("name=IronTowerDB")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<IronTowerDBContext, Configuration>());
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<IronTowerGame> Games { get; set; }

        public System.Data.Entity.DbSet<IronTower.API.Models.Business> Businesses { get; set; }

        public System.Data.Entity.DbSet<IronTower.API.Models.Floor> Floors { get; set; }

    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}