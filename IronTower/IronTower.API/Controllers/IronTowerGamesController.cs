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
        [HttpGet]
        public IHttpActionResult GetGames()
        {
            return Ok(db.Games.ToList());
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