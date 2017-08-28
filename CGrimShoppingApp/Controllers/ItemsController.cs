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
using System.IO;

namespace CGrimShoppingApp.Controllers
{
    public class ItemsController : Universal
    {
        public object system { get; private set; }

        // GET: Items
        public ActionResult Index()
        {
            var itemList = db.Items.ToList();
            return View(db.Items.ToList());         //returning data in a view
        }

        public ActionResult SearchResult(string search)
        {
            return View(db.Items.Where(i => i.Name.Contains(search) || i.Description.Contains(search)).ToList());
        }

        // GET: Items/Details/5
        public ActionResult Details(int? id)  //? can be null
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Items/Create
        [Authorize(Roles = "Admin")]       //Makes it to where has to be "Admin" to edit....Will kick back to login if not authorized
        public ActionResult Create()
        {                                  //Takes request to cotroller action, retrieves info from model, returns in view
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]                      //required fields
        public ActionResult Create([Bind(Include = "Id,CreationDate,UpdatedDate,Name,Price,MediaUrl,Description")] Item item, HttpPostedFileBase image)   //Accepting local disk file
        {
            if (image != null && image.ContentLength > 0) 
            {
                var ext = Path.GetExtension(image.FileName).ToLower();
                if (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif" && ext != ".bmp")
                ModelState.AddModelError("image", "Invaild Format.");
            }

            if (ModelState.IsValid)
            {
                if(image != null) {
                item.CreationDate = System.DateTime.Now;
                var filepath = "/Assets/New Images/";
                var absPath = Server.MapPath("~" + filepath);
                item.MediaUrl = filepath + image.FileName;
                image.SaveAs(Path.Combine(absPath, image.FileName));

                db.Items.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
                }
            }
            return View(item);
        }

        // GET: Items/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]                  //Expecting these inputs
        public ActionResult Edit([Bind(Include = "Id,CreationDate,UpdatedDate,Name,Price,MediaUrl,Description")] Item item, string mediaURl, HttpPostedFileBase image)
        {
            if (image != null && image.ContentLength > 0)
            {
                var ext = Path.GetExtension(image.FileName).ToLower();
                if (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif" && ext != ".bmp")
                    ModelState.AddModelError("image", "Invaild Format.");
            }

            if (ModelState.IsValid)
            {
                if(image != null) { 
                    
                    var filepath = "/Assets/New Images/";
                    var absPath = Server.MapPath("~" + filepath);
                    item.MediaUrl = filepath + image.FileName;
                    image.SaveAs(Path.Combine(absPath, image.FileName));
                    }
                
                    else
                    {
                    item.MediaUrl = mediaURl;
                    }
                item.UpdatedDate = System.DateTime.Now;
                db.Entry(item).State = EntityState.Modified;               //if valid allows to edit
                db.SaveChanges();
                return RedirectToAction("Index");
            }                          
            return View(item);          //if not valid returns to view item
        }

        // GET: Items/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);
            var absPath = Server.MapPath("~" + item.MediaUrl);
            System.IO.File.Delete(absPath);
            db.Items.Remove(item);
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

        //public ActionResult SearchItem(string search)
        //{
        //    String searchResult = "";
        //    String[] keyWord = searchItems.Split(' ');
        //    var itemList = db.Items.ToList();
        //    Item itemId = db.Items.Find();

        //    foreach (var item in itemList)
        //    {
        //        if (keyWord.Length == 0)
        //        {
        //            searchResult = View(item).ToList();
        //        }
        //    }
        //}
    }
}
