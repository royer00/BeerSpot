using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BeerSpot.DATA.EF;
using PagedList;

namespace BeerSpot.UI.MVC.Controllers
{
    public class BrewersController : Controller
    {
        private TheBeerSpotEntities2 db = new TheBeerSpotEntities2();

        // GET: Brewers
        //Implementing MVC Paging
        public ActionResult Index(string searchString, int page=1)
        {
            int pageSize = 5;
            var brewers = db.Brewers.ToList();

            if (searchString != null)
            {
                page = 1;
            }
           

            if (!string.IsNullOrEmpty(searchString))
            {
                brewers = db.Brewers.Where(x => x.Name.ToLower().Contains(searchString.ToLower())).ToList();
            }

            ViewBag.SearchFilter = searchString;

            return View(brewers.Where(x => x.IsActive).ToPagedList(page, pageSize));
            //return View(db.Brewers.Where(x => x.IsActive).ToList());
        }



        //Custom View for inactive brewers(soft delete)
        [Authorize(Roles = "Admin")]
        public ActionResult Inactive()
        {

            return View(db.Brewers.Where(x => !x.IsActive).ToList());
        }


        // GET: Brewers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brewer brewer = db.Brewers.Find(id);
            if (brewer == null)
            {
                return HttpNotFound();
            }
            return View(brewer);
        }

        // GET: Brewers/Create
        [Authorize(Roles ="Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Brewers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BrewerID,Name,Address,City,State,PostalCode,Country,Phone,IsActive")] Brewer brewer)
        {
            if (ModelState.IsValid)
            {
                db.Brewers.Add(brewer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(brewer);
        }

        // GET: Brewers/Edit/5
        [Authorize(Roles ="Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brewer brewer = db.Brewers.Find(id);
            if (brewer == null)
            {
                return HttpNotFound();
            }
            return View(brewer);
        }

        // POST: Brewers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BrewerID,Name,Address,City,State,PostalCode,Country,Phone,IsActive")] Brewer brewer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(brewer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(brewer);
        }

        // GET: Brewers/Delete/5
        [Authorize(Roles ="Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brewer brewer = db.Brewers.Find(id);
            if (brewer == null)
            {
                return HttpNotFound();
            }
            return View(brewer);
        }

        // POST: Brewers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Commenting out to add custom soft delete code
            //Brewer brewer = db.Brewers.Find(id);
            //db.Brewers.Remove(brewer);
            //db.SaveChanges();
            //return RedirectToAction("Index");
            Brewer brewer = db.Brewers.Find(id);
            brewer.IsActive = !brewer.IsActive;
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
