using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IronTower.API.Models
{
    [JsonObject(Title = "Floor")]
    public class CreateFloorVM
    {
        public int Business { get; set; }
       
    }

    [JsonObject(Title = "Game")]
    public class StartGameVM
    {
        public string Player { get; set; }
        public double TotalMoney { get; set; }
        
    }

   
}