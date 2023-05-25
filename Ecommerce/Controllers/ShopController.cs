using Ecommerce.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.Controllers
{
    public class ShopController : Controller
    {
        private Trainee_ShoppingCartEntities db = new Trainee_ShoppingCartEntities();
        List<cart> li = new List<cart>();
        /*private string strcart = "cart";*/

        // GET: Shop
        [AllowAnonymous]
        public ActionResult Home()
        {
            ViewBag.cat = db.Categories.Where(x => x.Active).ToList();
            /*ViewBag.min = db.Categories.Min(x => x.Products.Min(x=>x.Price)).(decimal)ToList();*/
            return View();
        }

        public ActionResult Shops()
        {
            return View();
        }
        
        public ActionResult Shop(int? page, int? catid, string sortOrder,  FormCollection fc)
        {
            var st = Convert.ToDecimal(fc["startprice"]);
            var lt = Convert.ToDecimal(fc["endprice"]);

            var nw = fc["select"];
            int pageSize = 4;
            int pageNumber = page ?? 1;
            ViewBag.cat = db.Categories.Where(x => x.Active).ToList();
            ViewBag.brand = db.Brands.ToList();
            ViewBag.col = db.Colors.ToList();
            /*var prod = db.Products.ToList();
            var d = prod.Max(x => x.Price);*/

            ViewBag.ds = db.Products.OrderByDescending(x => x.CreatedDate).Where(x => x.Active);
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.currntsort = sortOrder;
            ViewBag.datesort = sortOrder == "Date" ? "date_desc" : "Date";
            var prod = db.Products.Where(x => x.Active).OrderBy(x => x.Id);
            var d = prod.Max(x => x.Price);
            ViewBag.d = d;
            switch (sortOrder)
            {
                case "Date":
                    prod = db.Products.OrderByDescending(x => x.CreatedDate);
                    break;

                case "date_desc":
                    prod = db.Products.OrderByDescending(x => x.CreatedDate);
                    break;
                default:
                    prod = db.Products.OrderBy(x => x.Id);
                    break;
            }

            if (catid != null)
            {
                prod = db.Products.Where(x => x.CategoryId == catid & x.Active).OrderBy(x => x.Id);
            }
            else if (catid == null)
            {
                prod = db.Products.Where(x => x.Active).OrderBy(x => x.Id);
            }
           
            /* if (select != null)
             {
                 prod = db.Products.OrderByDescending(x=>x.CreatedDate).ToPagedList(pageNumber, pageSize);
             }*/
            if (st != 0)
            {
                prod = db.Products.Where(x => x.Price >= st & x.Price <= (lt) & x.Active).OrderBy(x => x.Id);
            }
            else if (lt != 0 & st != 0)
            {
                prod = db.Products.Where(x => x.Price >= st & x.Price <= lt & x.Active).OrderBy(x => x.Id);
            }

            /*  if (bn != null)
              {
                  prod = db.Products.Where(x => x.Brand.BrandName == bn & x.Active).OrderBy(x => x.Id).ToPagedList(pageNumber, pageSize);
              }*/

            /*  if (nw != null)
              {
                  prod = db.Products.OrderByDescending(x=>x.CreatedDate).Where(x=>x.CreatedDate<=DateTime.Now & x.Active).ToPagedList(pageNumber, pageSize);
              }*/

            /*else if (bn == null)
            {
                prod = db.Products.Where(x => x.Active).OrderBy(x => x.Id).ToPagedList(pageNumber, pageSize);
            }*/

            if (TempData["cart"] != null)
            {
                int x = 0;
                List<cart> li2 = TempData["cart"] as List<cart>;
                foreach (var item in li2)
                {
                    x += item.Total;
                }
                TempData["total"] = x;
                TempData["count"] = li2.Count();
            }
            TempData.Keep();

            return View(prod.ToPagedList(pageNumber,pageSize));
        }

        
        public ActionResult FilterProducts(string id, int? minimumPrice, int? maximumPrice, string catid,string clid)
        {

            ViewBag.cat = db.Categories.Where(x => x.Active).ToList();
            ViewBag.brand = db.Brands.ToList();
            ViewBag.col = db.Colors.ToList();


            var prod = db.Products.ToList();

            var d = prod.Where(x => x.Price >= minimumPrice && x.Price < maximumPrice + 1 && x.Active);

            if (catid != null && string.IsNullOrEmpty(id) && string.IsNullOrEmpty(clid))
            {
                prod = d.Where(x => x.CategoryId == Convert.ToInt32(catid) && x.Active).ToList();
            }
            
            else if (catid == null  && !string.IsNullOrEmpty(id) && clid==null)
            {
                var brandIds = id.Split(',').ToList();
                /*var catids = catid;*/
                prod = d.Where(x => brandIds.Contains(Convert.ToString(x.BrandId)) && x.Active).ToList();
            }
            else if (catid != null && !string.IsNullOrEmpty(id)&& clid==null)
            {
                var catids = catid;
                var brandIds = id.Split(',').ToList();

                /* prod = d.Where(x => catids.Contains(Convert.ToString(x.CategoryId)) && brandIds.Contains(Convert.ToString(x.BrandId))).ToList();*/
                prod = d.Where(x => x.CategoryId == Convert.ToInt32(catid) && brandIds.Contains(Convert.ToString(x.BrandId)) && x.Active).ToList();
            }

            else if (catid != null && !string.IsNullOrEmpty(id) && clid != null)
            {
                var catids = catid;
                var brandIds = id.Split(',').ToList();
                var clids = clid;

                /* prod = d.Where(x => catids.Contains(Convert.ToString(x.CategoryId)) && brandIds.Contains(Convert.ToString(x.BrandId))).ToList();*/
                prod = d.Where(x => x.CategoryId == Convert.ToInt32(catid) && brandIds.Contains(Convert.ToString(x.BrandId)) && x.ColorId == Convert.ToInt32(clid) && x.Active).ToList();
            }
            else if (catid == null && !string.IsNullOrEmpty(id) && clid != null)
            {
                
                var brandIds = id.Split(',').ToList();
                var clids = clid;

                /* prod = d.Where(x => catids.Contains(Convert.ToString(x.CategoryId)) && brandIds.Contains(Convert.ToString(x.BrandId))).ToList();*/
                prod = d.Where(x => brandIds.Contains(Convert.ToString(x.BrandId)) && x.ColorId == Convert.ToInt32(clid) && x.Active).ToList();



            }
            else
            {
                prod = d.ToList();
            }
            /*if (clid != null)
            {
                prod = d.Where(x => x.ColorId==Convert.ToInt32(clid) && x.Active).ToList();
            }
            else
            {
                prod = d.Where(x=> x.Active).ToList();
            }*/
            



            var offerHtml = RenderRazorViewToString("FilterProducts", prod,  this.ControllerContext);
            return Json(new { offerHtml = offerHtml }, JsonRequestBehavior.AllowGet);
        }

        
        public static string RenderRazorViewToString(string viewName, object model,  ControllerContext controllerContext)
        {

            controllerContext.Controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var ViewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
                var ViewContext = new ViewContext(controllerContext, ViewResult.View, controllerContext.Controller.ViewData, controllerContext.Controller.TempData, sw);
                ViewResult.View.Render(ViewContext, sw);
                ViewResult.ViewEngine.ReleaseView(controllerContext, ViewResult.View);
                return sw.GetStringBuilder().ToString();
            }


        }





        
        
        public ActionResult productdetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product pd = db.Products.Find(id);
            if (pd == null)
            {
                return HttpNotFound();
            }

            return View(pd);
        }
        
        public ActionResult Search(string search)
        {
            var prod = from s in db.Products select s;

            if (search != null)
            {
                prod = prod.Where(s => s.Title.Contains(search));

            }

            ViewBag.data = prod;

            return View(prod);
        }
        
        /*public ActionResult Addtocart(int id)
        {
            var qr = db.Products.Where(x => x.Id == id).SingleOrDefault();

            return View(qr);
        }*/


        /*[HttpPost]*/
        /*public ActionResult OrderNow(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            if (Session[strcart] == null)
            {
                List<cart> Iscart = new List<cart>
                {
                    new cart(db.Products.Find(id),1)
                };
                Session[strcart] = Iscart;

            }
            else
            {
                List<cart> Iscart = (List<cart>)Session[strcart];
                Iscart.Add(new cart(db.Products.Find(id), 1));
                Session[strcart] = Iscart;
            }
            
            return View("Index");
            

        }*/

        public ActionResult Index ()
        {
            return View();
        }


        [HttpPost]

        public ActionResult Addtocart(int id, int Qty)
        {
            Product p = db.Products.Find(id);
            cart c = new cart();
            c.id = Convert.ToInt32(p.Id);
            c.Name = p.Title;
            c.Image = p.ImagePath;
            c.Qty = Convert.ToInt32(Qty);
            /*c.Qty = 1;*/
            c.Price = Convert.ToInt32(p.Price);
            c.Total = c.Price * c.Qty;

            if (TempData["cart"] == null)
            {
                li.Add(c);
                TempData["cart"] = li;
            }
            else
            {
                List<cart> li2 = TempData["cart"] as List<cart>;
                int flag = 0;
                int x = 0;
                foreach (var item in li2)
                {
                    if (item.id == c.id)
                    {
                        item.Qty += c.Qty;
                        item.Total += c.Total;
                        flag = 1;
                        li2.Add(c);
                        x += item.Total;
                    }
                }
                if (flag == 0)
                {
                    li2.Add(c);
                }
                TempData["total"] = x;
                TempData["cart"] = li2;
            }
            TempData.Keep();
            return RedirectToAction("Shop");
        }

        public ActionResult plus(int id)
        {
            if (TempData["cart"] == null)
            {
                TempData.Remove("cart");
                TempData.Remove("total");
            }
            else
            {
                List<cart> li2 = TempData["cart"] as List<cart>;
                cart c = li2.Where(x => x.id == id).SingleOrDefault();
                c.Qty++;
                c.Total = c.Qty * c.Price;

                int s = 0;
                foreach (var item in li2)
                {
                    s += item.Total;
                }

                TempData["total"] = s;
                return RedirectToAction("Checkout");
            }
            return View();
        }

        public ActionResult minus(int id)
        {
            if (TempData["cart"] == null)
            {
                TempData.Remove("cart");
                TempData.Remove("total");
            }
            else
            {
                List<cart> li2 = TempData["cart"] as List<cart>;
                cart c = li2.Where(x => x.id == id).SingleOrDefault();
                if (c.Qty > 1)
                {
                    c.Qty--;
                    c.Total = c.Qty * c.Price;

                    int s = 0;
                    foreach (var item in li2)
                    {
                        s += item.Total;
                    }
                    TempData["total"] = s;

                }
                /*c.Qty--;
                c.Total = c.Qty * c.Price;
*/
/*                int s = 0;
                foreach (var item in li2)
                {
                    s += item.Total;
                }
                if (c.Qty == 0)
                {
                    li2.Remove(c);
                }
*/                
                return RedirectToAction("Checkout");
            }

            return View();
        }

        public ActionResult clearcart()
        {
            if (TempData["cart"] == null)
            {
                TempData.Remove("total");
                TempData.Remove("cart");
            }
            else
            {
                List<cart> li2 = TempData["cart"] as List<cart>;

                /*cart c = li2.OrderBy(x => x.id).ToString();*/

                if (li2 != null)
                {
                    TempData.Remove("cart");
                    TempData.Remove("total");
                }

                int s = 0;
                foreach (var item in li2)
                {
                    s += item.Total;
                }
                TempData["total"] = s;
                return RedirectToAction("Checkout");
            }

            return View();
        }


        public ActionResult remove(int id)
        {
            if (TempData["cart"] == null)
            {
                TempData.Remove("total");
                TempData.Remove("cart");
            }
            else
            {
                List<cart> li2 = TempData["cart"] as List<cart>;
                cart c = li2.Where(x => x.id == id).SingleOrDefault();

                li2.Remove(c);
                int s = 0;
                foreach (var item in li2)
                {
                    s += item.Total;
                }
                TempData["total"] = s;
                return RedirectToAction("Checkout");
            }

            return View();
        }


        public ActionResult Checkout()
        {
            TempData.Keep();
           
            return View();
        }

        [HttpPost]
        public ActionResult Checkouts(FormCollection fc)
        {
            int payment = Convert.ToInt32(fc["payment"]);

            
            if (payment == 1)
            {
                return RedirectToAction("Addresss");
            }
            else if(payment == 0)
            {
                return RedirectToAction("placeorder");
            }
            return View("Chekout");
        }
        
        public ActionResult placeorder()
        {
          
                TempData.Keep();
                return View();
           
            
        }

        public ActionResult Addresss()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ordt(Address ad)
        {
            if (TempData["cart"] != null)
            {


                Random randomobj = new Random();
                string transactionId = randomobj.Next(10000000, 100000000).ToString();
               
                Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient("rzp_test_ddzKCwd65Mvutz", "PLoY8TFedQ6lrOGRtZO4Z9iw");
                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", ad.amount * 100);
                options.Add("receipt", transactionId);
                options.Add("currency", "INR");
                options.Add("payment_capture", "0");



                Razorpay.Api.Order orderresponse = client.Order.Create(options);
                string orderId = orderresponse["id"].ToString();
                List<cart> li2 = TempData["cart"] as List<cart>;
                Order order = new Order();
                
                order.orderId = orderresponse.Attributes["id"];
                order.razorpaykey = "rzp_test_ddzKCwd65Mvutz";

                order.currency = "INR";
                order.OrderDate = DateTime.Now;
                order.FirstName = ad.FirstName;
                order.LastName = ad.LastName;
                order.CompanyName = ad.CompanyName;
                order.Email = ad.Email;
                order.Address = ad.Addresses;
                order.ZipCode = ad.ZipCode;
                order.Mobile = ad.Mobile;
                order.OrderAmount = ad.amount;

                db.Orders.Add(order);
                db.SaveChanges();

                foreach (var oi in li2)
                {
                    OrderDetail od = new OrderDetail();
                    /*{
                        OrderId = order.Id,
                        ProductId = oi.id,
                        Qty = oi.Qty,
                        UnitPrice = oi.Price
                    };
                    db.OrderDetails.Add(od);
                    db.SaveChanges();*/
                    od.OrderId = order.Id;
                    od.ProductId = oi.id;
                    od.UnitPrice = oi.Total;
                    od.Qty = oi.Qty;
                    db.OrderDetails.Add(od);
                    db.SaveChanges();
                    TempData.Remove("cart");
                    TempData.Remove("total");
                }
                return View("paymentpagesd", order);
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        
        public ActionResult placeorder(FormCollection fc)
        {
            
            if (ModelState.IsValid)
            {

                List<cart> li2 = TempData["cart"] as List<cart>;
                
                    Order order = new Order();                    
                    order.OrderDate = DateTime.Now;
                    order.FirstName = fc["fname"];
                    order.LastName = fc["lname"];
                    order.CompanyName = fc["company"];
                    order.Email = fc["email"];
                    order.Address = fc["Add"];
                    order.ZipCode = fc["zip"];
                    order.Mobile = fc["mobile"];                
                    order.OrderAmount = Convert.ToInt32(TempData["total"]);                
                    db.Orders.Add(order);
                    db.SaveChanges();
                

                foreach (var oi in li2)
                {
                    OrderDetail od = new OrderDetail();
                    /*{
                        OrderId = order.Id,
                        ProductId = oi.id,
                        Qty = oi.Qty,
                        UnitPrice = oi.Price
                    };
                    db.OrderDetails.Add(od);
                    db.SaveChanges();*/
                    od.OrderId = order.Id; 
                    od.ProductId = oi.id;
                    od.UnitPrice = oi.Total;
                    od.Qty = oi.Qty;
                    db.OrderDetails.Add(od);
                    db.SaveChanges();
                    TempData.Remove("cart");
                    TempData.Remove("total");
                }
                return View("placeorder", order);
            }
            /*TempData.Remove("total");*/
          return View();
           
        }

        
      
        
        public ActionResult Order()
        {
            var o = db.Orders.ToList();
            var od = db.OrderDetails.OrderByDescending(x => x.Id).ToList();

           
            ViewBag.o = o;
            ViewBag.od = od;

            return View();
        }

        
        public ActionResult od(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail ot = db.OrderDetails.Find(id);
            if (id == null)
            {
                return HttpNotFound();
            }
            return View(ot);


        }
        [HttpPost]
        public ActionResult Complete()
        {
            string paymentId = Request.Params["rzp_paymentid"];
            string orderId = Request.Params["rzp_orderid"];
            Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient("rzp_test_ddzKCwd65Mvutz", "PLoY8TFedQ6lrOGRtZO4Z9iw");
            Razorpay.Api.Payment payment = client.Payment.Fetch(paymentId);

            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", payment.Attributes["amount"]);
            Razorpay.Api.Payment paymentcapture = payment.Capture(options);

            string amt = paymentcapture.Attributes["amount"];

            if (paymentcapture.Attributes["status"] == "captured")
            {
                return RedirectToAction("Success");

            }
            else
            {
                return RedirectToAction("Failed");
            }

        }

        public ActionResult Success()
        {

            return View();
        }
        public ActionResult Failed()
        {
            return View();
        }

    }
}
