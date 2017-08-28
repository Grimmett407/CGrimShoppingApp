using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CGrimShoppingApp.Models;
using CGrimShoppingApp.Models.CodeFirst;
using Microsoft.AspNet.Identity;

namespace CGrimShoppingApp.Controllers
{
    public class OrdersController : Universal
    {
        // GET: Orders
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            return View(user.Orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order orderEntry = db.Orders.Find(id);
            if (orderEntry == null)
            {
                return HttpNotFound();
            }
            return View(orderEntry);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Address,City,State,ZipCode,Country,Phone,Total,OrderDate,CustomerId,OrderDetails")] Order order, decimal total)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                order.CustomerId = user.Id;
                order.OrderDate = System.DateTime.Now;
                order.Total = total;
                db.Orders.Add(order);
                db.SaveChanges();

                foreach(var item in user.CartItems.ToList())
                {
                    OrderItem orderitem = new OrderItem();
                    orderitem.ItemId = item.Item.Id;
                    orderitem.OrderId = order.Id;
                    orderitem.Quantity = item.Count;
                    orderitem.UnitPrice = item.Item.Price;
                    db.OrderItems.Add(orderitem);
                    db.CartItems.Remove(item);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order orderEntry = db.Orders.Find(id);
            if (orderEntry == null)
            {
                return HttpNotFound();
            }
            return View(orderEntry);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Address,City,State,ZipCode,Country,Phone,Total,OrderDate,CustomerId,OrderDetails")] Order orderEntry)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderEntry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(orderEntry);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order orderEntry = db.Orders.Find(id);
            if (orderEntry == null)
            {
                return HttpNotFound();
            }
            return View(orderEntry);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order orderEntry = db.Orders.Find(id);
            db.Orders.Remove(orderEntry);
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
