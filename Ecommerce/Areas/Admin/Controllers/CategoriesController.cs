using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ecommerce.Models;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private Trainee_ShoppingCartEntities db = new Trainee_ShoppingCartEntities();
       

        // GET: Admin/Categories
        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        // GET: Admin/Categories/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Admin/Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(HttpPostedFileBase file, Category category)
        {
            string filename = Path.GetFileName(file.FileName);
            /*string filename = Path.GetFileName(file.FileName);*/
            string extensions = Path.GetExtension(file.FileName);
            string path = Path.Combine(Server.MapPath("~/Content/upload/"), filename);
            category.ImagePath = "~/Content/upload/" + filename;
            category.CreatedBy = Convert.ToInt32(Session["AdminId"]);
            category.CreatedDate = DateTime.Now;

            if (extensions.ToLower() == ".jpg" || extensions.ToLower() == ".jpeg" || extensions.ToLower() == ".png")
            {
                if (file.ContentLength <= 2000000)
                {
                    db.Categories.Add(category);
                    if (db.SaveChanges() > 0)
                    {
                        file.SaveAs(path);
                        ViewBag.pd = "Data Added";
                    }
                }
            }


            return View(category);
        }



        /*public ActionResult Create( Category category)
        {

            string filename = Path.GetFileNameWithoutExtension(category.GetImageFile().FileName);
            string extension = Path.GetExtension(category.GetImageFile().FileName);
            category.ImagePath = "~/Content/upload/" + filename + extension;
            var img = category.ImagePath;
            var path = Path.Combine(Server.MapPath(img));
            category.GetImageFile().SaveAs(path);            
            category.CreatedBy = 1;
            category.CreatedDate = DateTime.Now;
            using (Trainee_ShoppingCartEntities db = new Trainee_ShoppingCartEntities())
            {
                db.Categories.Add(category);
                db.SaveChanges();

            }
            *//*ModelState.Clear();*/
        /* db.SaveChanges();*//*
        return View(category);



    }*/



        // GET: Admin/Categories/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            Session["Imagepath"] = category.ImagePath;

            if (category == null)
            {
                return HttpNotFound();
            }
            if (id == id)
            {
                category.ModifiedDate = DateTime.Now;
                category.ModifiedBy = category.CreatedBy;
            }
            return View(category);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase file, Category category)
        {

            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string filename = Path.GetFileName(file.FileName);
                    string extensions = Path.GetExtension(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/upload/"), filename);
                    category.ImagePath = "~/Content/upload/" + filename;


                    if (extensions.ToLower() == ".jpg" || extensions.ToLower() == ".jpeg" || extensions.ToLower() == ".png")
                    {
                        if (file.ContentLength <= 2000000)
                        {
                            db.Entry(category).State = EntityState.Modified;
                            string oldimg = Request.MapPath(Session["ImagePath"].ToString());
                            /*db.Products.Add(product);*/
                            if (db.SaveChanges() > 0)
                            {
                                file.SaveAs(path);
                                if (System.IO.File.Exists(oldimg))
                                {
                                    System.IO.File.Delete(oldimg);
                                }
                                ViewBag.pd = "Data Added";
                            }
                        }
                    }



                }
                /*product.ImagePath = Session["ImagePath"].ToString();*/
                db.Entry(category).State = EntityState.Modified;
                if (db.SaveChanges() > 0)
                {
                    return RedirectToAction("Index");
                }

                /* db.Entry(product).State = EntityState.Modified;
                 db.SaveChanges();
                 return RedirectToAction("Index");*/

            }
            else
            {
                category.ImagePath = Session["ImagePath"].ToString();
                db.Entry(category).State = EntityState.Modified;
                if (db.SaveChanges() > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            db.Entry(category).State = EntityState.Modified;
            db.SaveChanges();
           
            return View(category);


           /* if (category.ImagePath != null){
                string filename = Path.GetFileNameWithoutExtension(category.GetImageFile().FileName);
                string extension = Path.GetExtension(category.GetImageFile().FileName);
                category.ImagePath = "~/Content/upload/" + filename + extension;
                var path = Path.Combine(Server.MapPath("~/Content/upload/"), filename + extension);
                category.GetImageFile().SaveAs(path);

            }

            using (Trainee_ShoppingCartEntities db = new Trainee_ShoppingCartEntities())
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Index");*/
        }

        // GET: Admin/Categories/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (id == id)
            {
                category.Active = false;
            }
            return View(category);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Category category = db.Categories.Find(id);
            if (id != null)
            {
                category.Active = false;
            }
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
