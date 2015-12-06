using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IronTower.API.Models
{
    public class IronTowerGame
    {
        public int Id { get; set; }
        public string Player { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime Update { get; set; }
        public int TotalMoney { get; set; }  
        public int TotalResidents { get; set; }
        public int AvailableEmployees { get; set; }
        public int Capacity { get; set; }
        public ICollection<Floor> Floors { get; set; }
    }
    
}