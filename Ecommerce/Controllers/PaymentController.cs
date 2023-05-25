using Ecommerce.Models;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.Controllers
{
    public class PaymentController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Address ad)
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
            OrderModel orderModel = new OrderModel()
            {
                orderId = orderresponse.Attributes["id"],
                razorpayKey = "rzp_test_ddzKCwd65Mvutz",
                amount = ad.amount * 100,
                currency = "INR",
                FirstName = ad.FirstName,
                email = ad.Email,
                Mobile = ad.Mobile,
                Address = ad.Addresses,

            };

            return View("PaymentPage", orderModel);
        }

        public class OrderModel
        {
            public String orderId { get; set; }
            public string FirstName { get; set; }
           
            public string currency { get; set; }
            public string email { get; set; }
            public string Address { get; set; }

            public string Mobile { get; set; }

            public decimal amount { get; set; }
            public string razorpayKey { get; set; }


        }

       /* [HttpPost]
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
*/
        /*public ActionResult Success()
        {

            return View();
        }
        public ActionResult Failed()
        {
            return View();
        }*/
    }
}