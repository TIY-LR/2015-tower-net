﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace IronTower.API.Models
{
    public class Business
    {
        public Business()
        {
            AssociatedFloors = new Collection<Floor>();
        }
        public int Id { get; set; }
        public string Category { get; set; }
        public int Cost { get; set; }
        public int EarningsPerMinute { get; set; }
        public int NumberOfPeopleNeeded { get; set; }
        public int RateOfPopulation { get; set; }

        public virtual ICollection<Floor> AssociatedFloors { get; set; }
    }
}