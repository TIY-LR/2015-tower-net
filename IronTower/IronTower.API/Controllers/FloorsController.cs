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
        private const int MaxResidentsPerFloor = 5;
        private IronTowerDBContext db = new IronTowerDBContext();

        


        [HttpGet]
        [Route("api/games")]
        public IHttpActionResult UpdateTotal()
        {
            var currentGame = db.Games.Find(1);
            var rightNow = DateTime.Now;
            var secSinceLastGameUpdate = (rightNow - currentGame.Update).TotalSeconds;

            double moneyMade = 0;

            foreach (var floor in currentGame.Floors)
            {
                moneyMade += CalculateFloorMoney(floor, rightNow);

                CalculateFloorPopulation(floor, secSinceLastGameUpdate);

                floor.Update = rightNow;
            }

            currentGame.Update = rightNow;

            currentGame.TotalMoney += moneyMade;
            currentGame.TotalResidents = currentGame.Floors
                    .Where(x => x.Business.Category == "Residential")
                    .Sum(x => x.NumberOfEmployeesOrResidents);

            db.SaveChanges();

            return Ok(currentGame);
        }

        private static void CalculateFloorPopulation(Floor floor, double secSinceLastGameUpdate)
        {
            if (secSinceLastGameUpdate < 60)
                return;

            int numberOfUpdates = (int)secSinceLastGameUpdate / 60;

            if (floor.Business.Category == "Residential" && floor.NumberOfEmployeesOrResidents < MaxResidentsPerFloor)
            {
                floor.NumberOfEmployeesOrResidents += numberOfUpdates;
                if (floor.NumberOfEmployeesOrResidents > MaxResidentsPerFloor)
                    floor.NumberOfEmployeesOrResidents = MaxResidentsPerFloor;
            }

        }

        private static double CalculateFloorMoney(Floor floor, DateTime rightNow)
        {
            //listed total seconds
            var secondsSinceLastUpdate = (rightNow - floor.Update).TotalSeconds;
            //converted earnings/ min to seconds and multiply. Becomes money.
            var money = secondsSinceLastUpdate * (floor.Business.EarningsPerMinute / 60.0d);

            floor.TotalMoneyMade += money;

            return money;
        }


        [HttpPost]
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
        
        // GET: api/Floors
        [HttpGet]
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