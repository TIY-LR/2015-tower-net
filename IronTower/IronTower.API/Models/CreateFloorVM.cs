using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IronTower.API.Models
{
    public class CreateFloorVM
    {
        public int BusinessId { get; set; }
       
    }

    public class StartGameVM
    {
        public string Player { get; set; }
        public double TotalMoney { get; set; }
        
    }

   
}