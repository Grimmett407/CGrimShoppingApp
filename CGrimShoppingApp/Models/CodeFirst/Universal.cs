using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CGrimShoppingApp.Models.CodeFirst
{
    public class Universal : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                ViewBag.FirstName = user.FirstName;
                ViewBag.LastName = user.LastName;
                ViewBag.FullName = user.Fullname;
                ViewBag.CartItems = db.CartItems.AsNoTracking().Where(c => c.CustomerId == user.Id).ToList();       //Putting cart items into a list

                ViewBag.TotalCartItems = user.CartItems.Sum(c => c.Count);
                decimal total = 0;
                foreach (var item in db.CartItems.AsNoTracking().Where(c => c.CustomerId == user.Id).ToList())   //iterating through cart items pulling in the total price according to quanity
                {
                    total += item.Count * item.Item.Price;
                }

                ViewBag.CartTotal = total;

                base.OnActionExecuting(filterContext);
            }
        }


    }
}