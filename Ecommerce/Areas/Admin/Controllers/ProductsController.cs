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
    public class ProductsController : Controller
    {
        private Trainee_ShoppingCartEntities db = new Trainee_ShoppingCartEntities();

        // GET: Admin/Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Brand).Include(p => p.Category).Include(p => p.Color);
            return View(products.ToList());
        }

        // GET: Admin/Products/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            ViewBag.BrandId = new SelectList(db.Brands, "Id", "BrandName");
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName");
            ViewBag.ColorId = new SelectList(db.Colors, "Id", "ColorName");
            ViewBag.bnd = db.Brands.ToList();
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
  
        public ActionResult Create(HttpPostedFileBase file,  Product product)
        {
            string filename = Path.GetFileName(file.FileName);
            string extensions = Path.GetExtension(file.FileName);
            string path = Path.Combine(Server.MapPath("~/Content/upload/"),filename);
            product.ImagePath = "~/Content/upload/" + filename;
            product.CreatedBy = (long)Session["Id"];
            product.CreatedDate = DateTime.Now;

            if (extensions.ToLower()==".jpg"|| extensions.ToLower() == ".jpeg" || extensions.ToLower() == ".png")
            {
                if (file.ContentLength <= 2000000)
                {
                    db.Products.Add(product);
                    if (db.SaveChanges()> 0)
                    {
                        file.SaveAs(path);
                        ViewBag.pd = "Data Added";
                    }
                }
            }

            ViewBag.BrandId = new SelectList(db.Brands, "Id", "BrandName", product.BrandId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName", product.CategoryId);
            ViewBag.ColorId = new SelectList(db.Colors, "Id", "ColorName", product.ColorId);
            return View(product);

            
        }
        
        
        /*public ActionResult Create( Product product)
        {

            string filename = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
            string extension = Path.GetExtension(product.ImageFile.FileName);
            product.ImagePath = "~/Content/upload/" + filename + extension;
            var path = Path.Combine(Server.MapPath("~/Content/upload/"), filename+extension);
            product.ImageFile.SaveAs(path);
            product.CreatedBy = 1;
            product.CreatedDate = DateTime.Now;

            *//* if (ModelState.IsValid)
             {*//*
            using (Trainee_ShoppingCartEntities db = new Trainee_ShoppingCartEntities())
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BrandId = new SelectList(db.Brands, "Id", "BrandName", product.BrandId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName", product.CategoryId);
            ViewBag.ColorId = new SelectList(db.Colors, "Id", "ColorName", product.ColorId);
            return View(product);
        }
*/
        // GET: Admin/Products/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            Session["Imagepath"] = product.ImagePath;
            
            if (product == null)
            {
                return HttpNotFound();
            }
            if (id == id)
            {
                product.ModifiedDate = DateTime.Now;
                product.ModifiedBy = product.CreatedBy;
            }
            ViewBag.BrandId = new SelectList(db.Brands, "Id", "BrandName", product.BrandId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName", product.CategoryId);
            ViewBag.ColorId = new SelectList(db.Colors, "Id", "ColorName", product.ColorId);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase file, Product product)
        {
          if(ModelState.IsValid)
            {
                if (file != null)
                {
                    string filename = Path.GetFileName(file.FileName);
                    string extensions = Path.GetExtension(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/upload/"), filename);
                    product.ImagePath = "~/Content/upload/" + filename;
                    

                    if (extensions.ToLower() == ".jpg" || extensions.ToLower() == ".jpeg" || extensions.ToLower() == ".png")
                    {
                        if (file.ContentLength <= 2000000)
                        {
                            db.Entry(product).State = EntityState.Modified;
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
                db.Entry(product).State = EntityState.Modified;
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
                product.ImagePath = Session["ImagePath"].ToString();
                db.Entry(product).State = EntityState.Modified;
                if (db.SaveChanges() > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();
            ViewBag.BrandId = new SelectList(db.Brands, "Id", "BrandName", product.BrandId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName", product.CategoryId);
            ViewBag.ColorId = new SelectList(db.Colors, "Id", "ColorName", product.ColorId);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (id == id)
            {
                product.Active = false;
            }
            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Product product = db.Products.Find(id);
            if (id != null)
            {
                product.Active = false;
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
