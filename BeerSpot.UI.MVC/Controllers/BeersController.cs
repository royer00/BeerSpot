using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BeerSpot.DATA.EF;
using BeerSpot.UI.MVC.Models;

namespace BeerSpot.UI.MVC.Controllers
{
    public class BeersController : Controller
    {
        private TheBeerSpotEntities2 db = new TheBeerSpotEntities2();

        [HttpPost]
        public ActionResult AddToCart(int qty, int beerID)
        {
            //shell for local shopping cart
            Dictionary<int, ShoppingCartViewModel> shoppingCart = null;

            //Check the global shopping cart
            if (Session["cart"] != null)
            {
                //if items in global cart, reassign to local cart
                shoppingCart = (Dictionary<int, ShoppingCartViewModel>)Session["cart"];
            }
            //if Session cart empty
            else
            {
                //create a local version
                shoppingCart = new Dictionary<int, ShoppingCartViewModel>();
            }
            //get the product from the view
            Beer product = db.Beers.Where(x => x.BeerID == beerID).FirstOrDefault();
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                //beer is valid
                ShoppingCartViewModel item = new ShoppingCartViewModel(qty, product);
                //if the item is alread in the cart, just increase the qty
                if (shoppingCart.ContainsKey(product.BeerID))
                {
                    shoppingCart[product.BeerID].Qty += qty;
                }
                else//add item to the cart
                {
                    shoppingCart.Add(product.BeerID, item);
                }
                //update global cart from the local cart
                Session["cart"] = shoppingCart;

                Session["confirm"] = string.Format($"{qty} {product.Name}{(qty == 1 ? "" : "s")} " + $"{(qty == 1 ? "was" : "were")} added to your cart");

            }

            return RedirectToAction("Index", "ShoppingCart");

        }



        

        // GET: Beers
        public ActionResult Index()
        {
            var beers = db.Beers.Include(b => b.BeersStatu).Include(b => b.Brewer).Include(b => b.Style);
            return View(beers.Where(x => x.IsActive).ToList());
        }



        //Custom Controller Method for Inactive beers
        [Authorize(Roles ="Admin")]
        public ActionResult Inactive()
        {
            return View(db.Beers.Where(x => !x.IsActive).ToList());
        }


        // GET: Beers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beer beer = db.Beers.Find(id);
            if (beer == null)
            {
                return HttpNotFound();
            }
            return View(beer);
        }

        // GET: Beers/Create
        [Authorize(Roles ="Admin")]
        public ActionResult Create()
        {
            ViewBag.BeerStatusID = new SelectList(db.BeersStatus, "BeersStatusID", "Status");
            ViewBag.BrewerID = new SelectList(db.Brewers, "BrewerID", "Name");
            ViewBag.StyleID = new SelectList(db.Styles, "StyleID", "StyleName");
            return View();
        }

        // POST: Beers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin")]
        public ActionResult Create([Bind(Include = "BeerID,Name,StyleID,PricePerBeer,ABV,BrewerID,BeerStatusID,BeerImage,IsSeasonal,IsActive,Description,IsFeatured")]
        Beer beer, HttpPostedFileBase beerImage)
        {
            if (ModelState.IsValid)
            {
                #region File Upload
                //use a default image if none is provided
                string imgName = "noImg.png";

                if (beerImage != null)
                {
                    //get image and assign to variable
                    imgName = beerImage.FileName;

                    //declare and assign extension value
                    string ext = imgName.Substring(imgName.LastIndexOf('.'));

                    //declare a list of valid extenstion
                    string[] goodexts = {".jpeg", ".jpg", ".png", ".gif" };

                    //check the ext variable against goodexts
                    if (goodexts.Contains(ext.ToLower()) && (beerImage.ContentLength <= 4194304))
                    {
                        //if it is in the list, rename it using a guid
                        imgName = Guid.NewGuid() + ext;

                        //save to the webserver
                        beerImage.SaveAs(Server.MapPath("~/Content/assets/img/" + imgName));
                    }
                    else
                    {
                        imgName = "noImg.png";
                    }
                }
                //no matter what add the imageName to the object
                beer.BeerImage = imgName;

                #endregion

                db.Beers.Add(beer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BeerStatusID = new SelectList(db.BeersStatus, "BeersStatusID", "Status", beer.BeerStatusID);
            ViewBag.BrewerID = new SelectList(db.Brewers, "BrewerID", "Name", beer.BrewerID);
            ViewBag.StyleID = new SelectList(db.Styles, "StyleID", "StyleName", beer.StyleID);
            return View(beer);
        }

        // GET: Beers/Edit/5
        [Authorize(Roles ="Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beer beer = db.Beers.Find(id);
            if (beer == null)
            {
                return HttpNotFound();
            }
            ViewBag.BeerStatusID = new SelectList(db.BeersStatus, "BeersStatusID", "Status", beer.BeerStatusID);
            ViewBag.BrewerID = new SelectList(db.Brewers, "BrewerID", "Name", beer.BrewerID);
            ViewBag.StyleID = new SelectList(db.Styles, "StyleID", "StyleName", beer.StyleID);
            return View(beer);
        }

        // POST: Beers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin")]
        public ActionResult Edit([Bind(Include = "BeerID,Name,StyleID,PricePerBeer,ABV,BrewerID,BeerStatusID,BeerImage,IsSeasonal,IsActive,Description,IsFeatured")]
        Beer beer, HttpPostedFileBase beerImage)
        {
            if (ModelState.IsValid)
            {
                #region File Upload
                if (beerImage != null)
                {
                    //get image and assign to variable
                    string imgName = beerImage.FileName;

                    //declare and assign ext value
                    string ext = imgName.Substring(imgName.LastIndexOf('.'));

                    //declare and assign list of good extensions
                    string[] goodexts = { ".jpg", ".jpeg", ".gif", ".png" };

                    //Compare file extension against list
                    if (goodexts.Contains(ext.ToLower()) && (beerImage.ContentLength <= 4194304))
                    {
                        //if it's good, rename using a guid
                        imgName = Guid.NewGuid() + ext;

                        //save to the web server
                        beerImage.SaveAs(Server.MapPath("~/Content/assets/img/" + imgName));

                        //Make sure to not delete default image
                        if (beer.BeerImage != null && beer.BeerImage != "noImage.png")
                        {
                            //remove the original file from the Edit view
                            //System.IO.File.Delete(Server.MapPath("~/Content/assets/img" + Session["currentImage"].ToString()));
                        }
                        //only save if the image meets criteria imgageName to the object
                        beer.BeerImage = imgName;
                    }
                   
                }
                #endregion




                db.Entry(beer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BeerStatusID = new SelectList(db.BeersStatus, "BeersStatusID", "Status", beer.BeerStatusID);
            ViewBag.BrewerID = new SelectList(db.Brewers, "BrewerID", "Name", beer.BrewerID);
            ViewBag.StyleID = new SelectList(db.Styles, "StyleID", "StyleName", beer.StyleID);
            return View(beer);
        }

        // GET: Beers/Delete/5
        [Authorize(Roles ="Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beer beer = db.Beers.Find(id);
            if (beer == null)
            {
                return HttpNotFound();
            }
            return View(beer);
        }

        // POST: Beers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            //Commenting out code to change functionality to a "soft" delete
            //Beer beer = db.Beers.Find(id);
            //db.Beers.Remove(beer);
            //db.SaveChanges();
            //return RedirectToAction("Index");

            //Soft Delete
            Beer beer = db.Beers.Find(id);
            beer.IsActive = !beer.IsActive;
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
