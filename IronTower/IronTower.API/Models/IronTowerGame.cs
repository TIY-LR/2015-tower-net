using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace IronTower.API.Models
{
    [JsonObject(Title = "Game")]
    public class IronTowerGame
    {
        public IronTowerGame()
        {
            Floors = new Collection<Floor>();
        }
        public int Id { get; set; }
        public string Player { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime PopulationUpdate { get; set; }
        public double TotalMoney { get; set; }  



        public int TotalResidents { get; set; }
        public int AvailableEmployees { get; set; }
        public int Capacity { get; set; }



        public virtual ICollection<Floor> Floors { get; set; }
        public int PopulationCheckRate { get;  set; }
    }
    
}