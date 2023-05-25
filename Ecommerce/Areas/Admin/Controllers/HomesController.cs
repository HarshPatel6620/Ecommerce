using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.Areas.Admin.Controllers
{
    public class HomesController : Controller
    {
        private Trainee_ShoppingCartEntities db = new Trainee_ShoppingCartEntities();
        // GET: Admin/Homes
        public ActionResult Index()
        {
            Category cat = new Category
            {
                Id = db.Categories.Count()
            };

            Product pt = new Product
            {
                Id = db.Products.Count()
            };

            Brand bt = new Brand
            {
                Id = db.Brands.Count()
            };
            Color ct = new Color
            {
                Id = db.Colors.Count()
            };

            ViewBag.prod = pt;
            ViewBag.brand = bt;
            ViewBag.Data = cat;
            ViewBag.col = ct;
            return View();
        }
    }
}