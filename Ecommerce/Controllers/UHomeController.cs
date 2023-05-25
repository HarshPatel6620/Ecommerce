using Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.Controllers
{
    public class UHomeController : Controller
    {
        private Trainee_ShoppingCartEntities db = new Trainee_ShoppingCartEntities();


        // GET: UHome
        public ActionResult Index(string name, int?page)
        {
            var prod = from s in db.Products select s;
            //var Id = from x in db.Categories select x;
            if (!string.IsNullOrEmpty(name))
            {
                prod = prod.Where(s => s.Title.Contains(name));
            }
            /* var min = db.Products.Min(x => x.Price==x.CategoryId);*/


            if (TempData["cart"] != null)
            {
                int x = 0;
                List<cart> li2 = TempData["cart"] as List<cart>;
                foreach (var item in li2)
                {
                    var a = item.Total;
                    x += a;
                    x += item.Total;
                }
                TempData["total"] = x;
                TempData["count"] = li2.Count();
            }

            TempData.Keep();





            return View(db.Categories.Where(x => x.Active).ToList()) ;

        }
    }
    
}