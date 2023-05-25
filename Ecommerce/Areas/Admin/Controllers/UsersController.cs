using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Ecommerce.Models;

namespace Ecommerce.Areas.Admin.Controllers
{
  
    public class UsersController : Controller
    {
        private Trainee_ShoppingCartEntities db = new Trainee_ShoppingCartEntities();

        // GET: Users
      
        /*public ActionResult Index()
        {
            return View(db.Users.ToList());
        }*/
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]


        /*public ActionResult Login(Login objUser)
        {

            using(Trainee_ShoppingCartEntities db = new Trainee_ShoppingCartEntities())
            {
                var email = objUser.LoginId;
                var pass  = objUser.Password;
                var user = db.Users.FirstOrDefault();
                
                if(user.Equals(email) && user.Equals(pass))
                {
                    FormsAuthentication.SetAuthCookie(objUser.LoginId, false);
                    return RedirectToAction("Index", "Homes");
                }                               
            }
            ModelState.AddModelError("", "Invalid");
            return View();


        }*/

       public ActionResult Login(Login objUser)
        {
            
            using (Trainee_ShoppingCartEntities db = new Trainee_ShoppingCartEntities())
            {
                var obj = db.Users.Where(a => a.Email.Equals(objUser.LoginId) && a.Password.Equals(objUser.Password)).FirstOrDefault();

                if (obj != null)
                {
                    FormsAuthentication.SetAuthCookie(objUser.LoginId, false);
                    Session["Id"] = obj.Id;
                    return RedirectToAction("Index", "Homes");
                }

                ModelState.AddModelError("", "Invalid");
                return View();
            }

           


        }

       /* public ActionResult Login(string email, string password)
        {
            var us = db.Users.Where(x => x.Email == email && x.Password == password).FirstOrDefault();
            if (us != null)
            {
                return RedirectToAction("Index", "Homes");
            }

            return View();
        }*/
        public ActionResult LoggedIn()
        {
            object obj = Session["Id"];
            if (obj != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }

        }
        
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Users");
        }
    }
}
