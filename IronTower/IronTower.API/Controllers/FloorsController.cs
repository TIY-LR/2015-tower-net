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

        [HttpPost]
        public IHttpActionResult CreateFloor(CreateFloorVM data)
        {
            var currentGame = db.Games.OrderByDescending(g => g.DateCreated).FirstOrDefault();
            if (currentGame == null)
            {
                return BadRequest($"Latest Game was not found");
            }

            //if (currentGame.AvailableEmployees < currentGame)
            //{

            //}
            var business = db.Businesses.Find(data.Business);
            if (business == null)
            {
                return BadRequest($"Business Id of {data.Business} was not found");
            }
            int nextfloornum = 1;
            if (currentGame.Floors.Any())
            {
                nextfloornum = currentGame.Floors.Max(x => x.FloorNumber) + 1;
            }

            Floor floor = new Floor
            {
                DateCreated = DateTime.Now,
                Update = DateTime.Now,
                Game = currentGame,
                Business = business,
                FloorNumber = nextfloornum
            };
            db.Floors.Add(floor);

            if (currentGame.TotalMoney < floor.Business.Cost)
            {
                return BadRequest("Bro, you do not have enough dough!");
            }

            // Attempting to subtract cost from total money made
            currentGame.TotalMoney -= floor.Business.Cost;

            switch (floor.Business.Category)
            {
                case "Residential":
                    currentGame.Capacity += 5;
                    break;
                default:
                    currentGame.AvailableEmployees -= 3;
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