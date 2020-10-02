using BeerSpot.DATA.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BeerSpot.UI.MVC.Models
{
    public class ShoppingCartViewModel
    {
        [Range(1,byte.MaxValue,ErrorMessage ="*Must be between 1 and 255")]
        public int Qty { get; set; }
        public Beer Product { get; set; }

        public ShoppingCartViewModel(int qty, Beer product)
        {
            Qty = qty;
            Product = product;
        }

    }
}