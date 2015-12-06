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
    [Route("api/games")]
    public class IronTowerGamesController : ApiController
    {
        
        private IronTowerDBContext db = new IronTowerDBContext();

        [HttpPost]
        public IHttpActionResult StartGame(StartGameVM model)
        {


            IronTowerGame newGame = new IronTowerGame()
            {
                Player = model.Player,
                DateCreated = DateTime.Now,
                Update = DateTime.Now,
                TotalMoney = model.TotalMoney               

            };
            db.Games.Add(newGame);
            db.SaveChanges();
            return Ok();
        }

        [HttpGet]
        public IHttpActionResult UpdateTotal()
        {
            var currentGame = db.Games.OrderByDescending(g=>g.DateCreated).FirstOrDefault();
            if (currentGame == null)
            {
                return NotFound();
            }
            var rightNow = DateTime.Now;
            var secSinceLastGameUpdate = (rightNow - currentGame.Update).TotalSeconds;

            double moneyMade = 0;

            foreach (var floor in currentGame.Floors)
            {
                moneyMade += CalculateFloorMoney(floor, rightNow);

                CalculateFloorPopulation(floor, secSinceLastGameUpdate, currentGame.Capacity);

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


        private static void CalculateFloorPopulation(Floor floor, double secSinceLastGameUpdate, int maxResidentsPerFloor)
        {
            if (secSinceLastGameUpdate < 60)
                return;

            int numberOfUpdates = (int)secSinceLastGameUpdate / 60;

            if (floor.Business.Category == "Residential" && floor.NumberOfEmployeesOrResidents < maxResidentsPerFloor)
            {
                floor.NumberOfEmployeesOrResidents += numberOfUpdates;
                if (floor.NumberOfEmployeesOrResidents > maxResidentsPerFloor)
                    floor.NumberOfEmployeesOrResidents = maxResidentsPerFloor;
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


       

        // GET: api/IronTowerGames/5
        [ResponseType(typeof(IronTowerGame))]
        public IHttpActionResult GetIronTowerGame(int id)
        {
            IronTowerGame ironTowerGame = db.Games.Find(id);
            if (ironTowerGame == null)
            {
                return NotFound();
            }

            return Ok(ironTowerGame);
        }

        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool IronTowerGameExists(int id)
        {
            return db.Games.Count(e => e.Id == id) > 0;
        }
    }
}