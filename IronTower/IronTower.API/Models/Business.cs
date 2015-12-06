using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IronTower.API.Models
{
    public class Business
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public int Cost { get; set; }
        public int EarningsPerMinute { get; set; }
        public int NumberOfPeopleNeeded { get; set; }
        public int RateOfPopulation { get; set; }

        public ICollection<Floor> AssociatedFloors { get; set; }
    }
}