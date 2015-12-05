using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IronTower.API.Models
{
    public class Business
    {
        public int Id { get; set; }
        public Category Category { get; set; }
        public double Cost { get; set; }
        public double EarningsPerMinute { get; set; }
        public int NumberOfPeopleNeeded { get; set; }

        public ICollection<Floor> AssociatedFloors { get; set; }
    }
}