using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IronTower.API.Models
{
    public class Floor
    {
        public int Id { get; set; }
        public virtual Business Business { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime Update { get; set; }
        public virtual IronTowerGame Game { get; set; }

        public int NumberOfEmployeesOrResidents { get; set; }
        public int FloorNumber { get; set; }
        public double TotalMoneyMade { get; set; }
    }

   
}