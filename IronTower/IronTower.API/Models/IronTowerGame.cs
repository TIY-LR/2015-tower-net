﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace IronTower.API.Models
{
    public class IronTowerGame
    {
        public IronTowerGame()
        {
            Floors = new Collection<Floor>();
        }
        public int Id { get; set; }
        public string Player { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime Update { get; set; }
        public int TotalMoney { get; set; }  
        public int TotalResidents { get; set; }
        public int AvailableEmployees { get; set; }
        public int Capacity { get; set; }
        public virtual ICollection<Floor> Floors { get; set; }
    }
    
}