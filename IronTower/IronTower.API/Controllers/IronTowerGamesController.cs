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
                PopulationUpdate = DateTime.Now,
                TotalMoney = model.TotalMoney,
                PopulationCheckRate = model.PopulationCheckRate
                
            };
            db.Games.Add(newGame);
            db.SaveChanges();
            return Ok(newGame);
        }

        [HttpGet]
        public IHttpActionResult UpdateTotal()
        {
            var currentGame = db.Games.OrderByDescending(g => g.DateCreated).FirstOrDefault();
            if (currentGame == null)
            {
                return NotFound();
            }
            var rightNow = DateTime.Now;

            CalculateGameMoney(currentGame, rightNow);

            CalculateFloorPopulations(currentGame, rightNow);

            CalculateGamePopulation(currentGame, rightNow);

            db.SaveChanges();

            return Ok(currentGame);
        }

        private static void CalculateGamePopulation(IronTowerGame currentGame, DateTime rightNow)
        {
            var apartments = currentGame.Floors.FilterFloors(true);
            var businesses = currentGame.Floors.FilterFloors(false);

            currentGame.TotalResidents = apartments.Sum(x => x.NumberOfEmployeesOrResidents);
            currentGame.Capacity = apartments.Count() * 5;
            currentGame.AvailableEmployees = currentGame.TotalResidents - (businesses.Count() * 3);

        }

        public static void CalculateFloorPopulations(IronTowerGame currentGame, DateTime rightNow)
        {
            var secSinceLastGameUpdate = (rightNow - currentGame.PopulationUpdate).TotalSeconds;
            int SpeedOfPopUpdateInSeconds = currentGame.PopulationCheckRate;

            if (secSinceLastGameUpdate < SpeedOfPopUpdateInSeconds)
                return;

            foreach (var floor in currentGame.Floors.FilterFloors(true))
            {
                CalculateFloorPopulation(floor, secSinceLastGameUpdate, 5, SpeedOfPopUpdateInSeconds);
            }


            currentGame.PopulationUpdate = rightNow;

        }

        private static void CalculateGameMoney(IronTowerGame currentGame, DateTime rightNow)
        {
            double moneyMade = 0;
            foreach (var floor in currentGame.Floors.FilterFloors(false))
            {
                moneyMade += CalculateFloorMoney(floor, rightNow);
                floor.Update = rightNow;
            }
            currentGame.TotalMoney += moneyMade;
        }

        private static void CalculateFloorPopulation(Floor floor, double secSinceLastGameUpdate, int maxResidentsPerFloor, int speedOfPopUpdateInSeconds)
        {
            int numberOfUpdates = (int)(secSinceLastGameUpdate / speedOfPopUpdateInSeconds) * floor.Business.RateOfPopulation;
            if (numberOfUpdates < 1)
                return;

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

    public static class GameHelpers
    {
        public static IEnumerable<Floor> FilterFloors(this IEnumerable<Floor> source, bool residentialOnly)
        {
            if (residentialOnly)
                return source.Where(x => x.Business.Category == "Residential");
            else
                return source.Where(x => x.Business.Category != "Residential");

        }

    }
}