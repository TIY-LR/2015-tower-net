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

        // GET: api/Floors
        public IQueryable<Floor> GetFloors()
        {
            return db.Floors;
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

        // PUT: api/Floors/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFloor(int id, Floor floor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != floor.Id)
            {
                return BadRequest();
            }

            db.Entry(floor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FloorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Floors
        [ResponseType(typeof(Floor))]
        public IHttpActionResult PostFloor(Floor floor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newFloor = new Floor()
            {
                Business = floor.Business,
                DateCreated = DateTime.Now,
                Update = floor.Update,
                Game = floor.Game

            };

            db.Floors.Add(newFloor);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = floor.Id }, newFloor);
        }

        // DELETE: api/Floors/5
        [ResponseType(typeof(Floor))]
        public IHttpActionResult DeleteFloor(int id)
        {
            Floor floor = db.Floors.Find(id);
            if (floor == null)
            {
                return NotFound();
            }

            db.Floors.Remove(floor);
            db.SaveChanges();

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