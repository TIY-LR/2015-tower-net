using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using IronTower.API.Models;

namespace IronTower.API.Controllers
{
    public class FloorsController : ApiController
    {
        private IronTowerDBContext db = new IronTowerDBContext();

        public int RevenueCalculation()
        {
            return db.Floors.Find().Business.EarningsPerMinute * db.Floors.Count();

            //var seconds = (int) System.TimeSpan.FromSeconds(60).Ticks/1000;

            //return ((int) earningsByFloor * seconds);
        }

        public void PopulationIncreasesEveryMinute()
        {
            var category = db.Floors.Find().Business.Category;
            var num = db.Floors.Find().NumberOfEmployeesOrResidents;
            if (category == "Residential" && (num < 5))
            {
                num += 1;
            }
        }


        [HttpPost]
        [Route("api/addfloor")]
        public IHttpActionResult CreateFloor(CreateFloorVM data)
        {
            var game = db.Games.Find(1);
            var business = db.Businesses.Find(data.BusinessId);
            var nextfloornum = db.Floors.Where(x => x.Game == game).Max(x => x.Id /*Floor ID*/) + 1;
            Floor floor = new Floor
            {
                DateCreated = DateTime.Now,
                Update = DateTime.Now,
                Game = game,
                Business = business,
                Id = nextfloornum,
                FloorRevenue = 0
            };
            
            // Attempting to subtract cost from total money made

            var cost = floor.Business.Cost;

            var total = db.Games.Where(x => x.Id == game.Id).First().TotalMoney;

            //Subtract that money
            total -= cost;
            
            //Subtract from available employees
            if (floor.Business.Category != "Residential")
            {
                db.Games.Find().AvailableEmployees -= 3;
                
            }

            db.Games.Find().Capacity += 1;

            //Add and save changes
            db.Floors.Add(floor);
            db.SaveChanges();
            return Ok(floor);
        }

        [HttpGet]
        [Route("api/updatetotal")]
        public IHttpActionResult UpdateTotal(Floor floor)
        {
            //Money increase
            var total = db.Games.Where(x => x.Id == floor.Game.Id).First().TotalMoney;
            var rev = RevenueCalculation();
            rev += total;

            //Population increase
            PopulationIncreasesEveryMinute();
            db.SaveChanges();
            return Ok();
        }

        // GET: api/Floors
        [HttpGet]
        [Route("api/floors")]
        public IHttpActionResult GetFloors()
        {
            return Ok(db.Floors.ToList());
        }

        // GET: api/Floors/5
        [ResponseType(typeof(Floor))]
        public IHttpActionResult GetFloor(int id)
        {
            Floor floor = db.Floors.Find(id);
            if (floor == null)
            {
                return NotFound();
            }

            return Ok(floor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FloorExists(int id)
        {
            return db.Floors.Count(e => e.Id == id) > 0;
        }
    }
}