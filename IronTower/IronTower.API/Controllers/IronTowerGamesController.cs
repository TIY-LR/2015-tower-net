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
    public class IronTowerGamesController : ApiController
    {
        private IronTowerDBContext db = new IronTowerDBContext();

        // GET: api/IronTowerGames
        public IQueryable<IronTowerGame> GetGames()
        {
            return db.Games;
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

        // PUT: api/IronTowerGames/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutIronTowerGame(int id, IronTowerGame ironTowerGame)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ironTowerGame.Id)
            {
                return BadRequest();
            }

            db.Entry(ironTowerGame).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IronTowerGameExists(id))
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

        // POST: api/IronTowerGames
        [ResponseType(typeof(IronTowerGame))]
        public IHttpActionResult PostIronTowerGame(IronTowerGame ironTowerGame)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Games.Add(ironTowerGame);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = ironTowerGame.Id }, ironTowerGame);
        }

        // DELETE: api/IronTowerGames/5
        [ResponseType(typeof(IronTowerGame))]
        public IHttpActionResult DeleteIronTowerGame(int id)
        {
            IronTowerGame ironTowerGame = db.Games.Find(id);
            if (ironTowerGame == null)
            {
                return NotFound();
            }

            db.Games.Remove(ironTowerGame);
            db.SaveChanges();

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