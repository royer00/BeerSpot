using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BeerSpot.UI.MVC.Models;
using BeerSpot.DATA.EF;

namespace BeerSpot.UI.MVC.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        public ActionResult Index()
        {
            //Create a local verson of the shopping cart from the session cart
            //if the value is null or cound is 0, create and empty instance and provide no cart items verbiage
            var shoppingCart = (Dictionary<int, ShoppingCartViewModel>)Session["cart"];
            if (shoppingCart == null || shoppingCart.Count == 0)
            {
                shoppingCart = new Dictionary<int, ShoppingCartViewModel>();
                ViewBag.Message = "There are no beers in your cart";
            }
            //if the cart isn't null and count > 0, null the messaging
            else
            {
                ViewBag.Message = null;
            }
            return View(shoppingCart);
        }

        public ActionResult UpdateCart(int beerID, int qty)
        {
            #region Care for 0 qty
            //if the qty is 0 from the update, remove item from cart
            if (qty == 0)
            {
                RemoveFromCart(beerID);
                return RedirectToAction("Index");
            }
            #endregion

            //retreive the cart from session and assign it to the local dictionary
            Dictionary<int, ShoppingCartViewModel> shoppingCart = (Dictionary<int, ShoppingCartViewModel>)Session["cart"];
            //update the qty in the local storage
            shoppingCart[beerID].Qty = qty;
            //return local cart to session
            Session["cart"] = shoppingCart;
            //logic to display a message if the update to NO items in their cart
            if (shoppingCart.Count == 0)
            {
                ViewBag.Message = "There are no beers in your Cart";
            }
            return RedirectToAction("Index");

        }

        public ActionResult RemoveFromCart(int id)
        {
            //cart out of session and into the local
            Dictionary<int, ShoppingCartViewModel> shoppingCart = (Dictionary<int,ShoppingCartViewModel>)Session["cart"];
            //call the Remove() from the dictionary class
            shoppingCart.Remove(id);
            //null the session to hide the cart link when session is empty
            //null the cart if empty
            if (shoppingCart.Count == 0)
            {
                Session["cart"] = null;
            }
            //redirect back to the index action
            return RedirectToAction("Index");

        }

    }
}