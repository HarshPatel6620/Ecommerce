using Ecommerce.Models;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace Ecommerce.Controllers
{
    public class HomeController : Controller
    {
        private Trainee_ShoppingCartEntities db = new Trainee_ShoppingCartEntities();
        public ActionResult Index()
        {

            return View(db.Products.ToList());
        }
        [HttpGet]

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Login(Login objUser)
        {
            /*            if (ModelState.IsValid)
                        {*/
            using (Trainee_ShoppingCartEntities db = new Trainee_ShoppingCartEntities())
            {
                var obj = db.Users.Where(a => a.Email.Equals(objUser.LoginId) && a.Password.Equals(objUser.Password)).FirstOrDefault();
                
                if (obj != null)
                {
                    FormsAuthentication.SetAuthCookie(objUser.LoginId, false);
                    /*Session["User"] = obj.Id.ToString();
                    Session["UserName"] = obj.Email.ToString();*/
                    return RedirectToAction("Shop", "Shop");
                }
                ModelState.AddModelError("", "Invalid");
                return View();
            }

            /*  }*/


        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","UHome");
        }


    }
}