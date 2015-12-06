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
    public class BusinessesController : ApiController
    {
        private IronTowerDBContext db = new IronTowerDBContext();

        // GET: api/Businesses
        [HttpGet]
        public IHttpActionResult GetBusinesses()
        {
            return Ok(db.Businesses.ToList());
        }

        // GET: api/Businesses/5
        [ResponseType(typeof(Business))]
        public IHttpActionResult GetBusiness(int id)
        {
            Business business = db.Businesses.Find(id);
            if (business == null)
            {
                return NotFound();
            }

            return Ok(business);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BusinessExists(int id)
        {
            return db.Businesses.Count(e => e.Id == id) > 0;
        }
    }
}