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
                FloorNumber = data.FloorNumber
            };
            db.Floors.Add(floor);

            if (game.TotalMoney < floor.Business.Cost)
            {
                return BadRequest("Bro, you do not have enough dough!");
            }

            // Attempting to subtract cost from total money made
            game.TotalMoney -= floor.Business.Cost;

            switch (floor.Business.Category)
            {
                case "Residential":
                    game.Capacity += 5;
                    break;
                default:
                    game.AvailableEmployees -= 3;
                    break;
            }
          
            //Add and save changes
            db.SaveChanges();
           
            return Ok(floor);
        }

        [HttpGet]
        [Route("api/games")]
        public IHttpActionResult UpdateTotal(Floor floor, int floorid)
        {
            //Money increase
            var total = db.Games.Where(x => x.Id == floorid).First().TotalMoney;
            var rev = RevenueCalculation();
            rev += total;

            //Population increase
            PopulationIncreasesEveryMinute();
            db.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("api/totalscore")]
        public IHttpActionResult TotalScore()
        {
            return Ok(db.Games.Find().TotalMoney);
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