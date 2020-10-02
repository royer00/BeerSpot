using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BeerSpot.DATA.EF;

namespace BeerSpot.UI.MVC.Controllers
{
    public class BeersContainersController : Controller
    {
        private TheBeerSpotEntities2 db = new TheBeerSpotEntities2();

        // GET: BeersContainers
        [Authorize(Roles ="Admin, Manager")]
        public ActionResult Index()
        {
            var beersContainers = db.BeersContainers.Include(b => b.Beer).Include(b => b.Container);
            return View(beersContainers.ToList());
        }
        [Authorize(Roles = "Admin, Manager")]
        // GET: BeersContainers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BeersContainer beersContainer = db.BeersContainers.Find(id);
            if (beersContainer == null)
            {
                return HttpNotFound();
            }
            return View(beersContainer);
        }

        // GET: BeersContainers/Create
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Create()
        {
            ViewBag.BeerID = new SelectList(db.Beers, "BeerID", "Name");
            ViewBag.ContainerID = new SelectList(db.Containers, "ContainerID", "Type");
            return View();
        }

        // POST: BeersContainers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Create([Bind(Include = "BeersContainersID,BeerID,ContainerID")] BeersContainer beersContainer)
        {
            if (ModelState.IsValid)
            {
                db.BeersContainers.Add(beersContainer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BeerID = new SelectList(db.Beers, "BeerID", "Name", beersContainer.BeerID);
            ViewBag.ContainerID = new SelectList(db.Containers, "ContainerID", "Type", beersContainer.ContainerID);
            return View(beersContainer);
        }

        // GET: BeersContainers/Edit/5
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BeersContainer beersContainer = db.BeersContainers.Find(id);
            if (beersContainer == null)
            {
                return HttpNotFound();
            }
            ViewBag.BeerID = new SelectList(db.Beers, "BeerID", "Name", beersContainer.BeerID);
            ViewBag.ContainerID = new SelectList(db.Containers, "ContainerID", "Type", beersContainer.ContainerID);
            return View(beersContainer);
        }

        // POST: BeersContainers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BeersContainersID,BeerID,ContainerID")] BeersContainer beersContainer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(beersContainer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BeerID = new SelectList(db.Beers, "BeerID", "Name", beersContainer.BeerID);
            ViewBag.ContainerID = new SelectList(db.Containers, "ContainerID", "Type", beersContainer.ContainerID);
            return View(beersContainer);
        }

        // GET: BeersContainers/Delete/5
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BeersContainer beersContainer = db.BeersContainers.Find(id);
            if (beersContainer == null)
            {
                return HttpNotFound();
            }
            return View(beersContainer);
        }

        // POST: BeersContainers/Delete/5
        [Authorize(Roles = "Admin, Manager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BeersContainer beersContainer = db.BeersContainers.Find(id);
            db.BeersContainers.Remove(beersContainer);
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
