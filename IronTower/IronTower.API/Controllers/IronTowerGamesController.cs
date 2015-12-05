using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IronTower.API.Models;

namespace IronTower.API.Controllers
{
    public class IronTowerGamesController : Controller
    {
        private IronTowerDBContext db = new IronTowerDBContext();

        // GET: IronTowerGames
        public ActionResult Index()
        {
            return View(db.Games.ToList());
        }

        // GET: IronTowerGames/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IronTowerGame ironTowerGame = db.Games.Find(id);
            if (ironTowerGame == null)
            {
                return HttpNotFound();
            }
            return View(ironTowerGame);
        }

        // GET: IronTowerGames/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: IronTowerGames/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Player,DateCreated,Update,TotalMoney")] IronTowerGame ironTowerGame)
        {
            if (ModelState.IsValid)
            {
                db.Games.Add(ironTowerGame);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ironTowerGame);
        }

        // GET: IronTowerGames/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IronTowerGame ironTowerGame = db.Games.Find(id);
            if (ironTowerGame == null)
            {
                return HttpNotFound();
            }
            return View(ironTowerGame);
        }

        // POST: IronTowerGames/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Player,DateCreated,Update,TotalMoney")] IronTowerGame ironTowerGame)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ironTowerGame).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ironTowerGame);
        }

        // GET: IronTowerGames/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IronTowerGame ironTowerGame = db.Games.Find(id);
            if (ironTowerGame == null)
            {
                return HttpNotFound();
            }
            return View(ironTowerGame);
        }

        // POST: IronTowerGames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IronTowerGame ironTowerGame = db.Games.Find(id);
            db.Games.Remove(ironTowerGame);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
