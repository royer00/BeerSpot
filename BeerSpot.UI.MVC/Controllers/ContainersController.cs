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
    public class ContainersController : Controller
    {
        private TheBeerSpotEntities2 db = new TheBeerSpotEntities2();

        // GET: Containers
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(db.Containers.ToList());
        }


        #region AJAX
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult AjaxDelete(int id)
        {
            //container from the db
            Container con = db.Containers.Find(id);

            //remove container from EF
            db.Containers.Remove(con);

            //save changes to db
            db.SaveChanges();

            //user message 
            var message = $"Deleted the following container from the database: {con.Type}";

            //return json result
            return Json(
                new
                {
                    id = id,
                    message = message

                }
                );
        }

        //AJAX Details
        [HttpGet]
        public PartialViewResult ContainerDetails(int id)
        {
            //retrieve the container by id
            Container con = db.Containers.Find(id);
            //return the partial view with the container object
            return PartialView(con);
        }

        //AJAX Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AjaxCreate(Container container)
        {
            db.Containers.Add(container);
            db.SaveChanges();
            return Json(container);
        }

        //AJAX Edit GET
        public PartialViewResult ContainerEdit(int id)
        {
            Container container = db.Containers.Find(id);
            return PartialView(container);
        }

        //AJAX Edit POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AjaxEdit(Container container)
        {
            db.Entry(container).State = EntityState.Modified;
            db.SaveChanges();
            return Json(container);
        }

        #endregion


        // GET: Containers/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Container container = db.Containers.Find(id);
            if (container == null)
            {
                return HttpNotFound();
            }
            return View(container);
        }

        // GET: Containers/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Containers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "ContainerID,Type")] Container container)
        {
            if (ModelState.IsValid)
            {
                db.Containers.Add(container);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(container);
        }

        // GET: Containers/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Container container = db.Containers.Find(id);
            if (container == null)
            {
                return HttpNotFound();
            }
            return View(container);
        }

        // POST: Containers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "ContainerID,Type")] Container container)
        {
            if (ModelState.IsValid)
            {
                db.Entry(container).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(container);
        }

        // GET: Containers/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Container container = db.Containers.Find(id);
            if (container == null)
            {
                return HttpNotFound();
            }
            return View(container);
        }

        // POST: Containers/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Container container = db.Containers.Find(id);
            db.Containers.Remove(container);
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
