using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IronTower.API.Models
{
    public class Floor
    {
        public int Id { get; set; }
        public Business Business { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime Update { get; set; }
        public IronTowerGame Game { get; set; }
    }
}