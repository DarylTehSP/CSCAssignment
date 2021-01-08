using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Task6.Models;

namespace Task6.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
           /// ViewBag.Title = "Home Page";

            return View();
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Charge()
        {
            ViewBag.Message = "Learn how to process payments with Stripe";

            return View();
        }

        //public ActionResult Charge()
        //{
        //    ViewBag.Message = "Learn how to process payments with Stripe";

        //    return View(new StripeChargeModel());
        //}

        [HttpPost]
        public string Charge(StripeChargeModel model)
        {
            if (!ModelState.IsValid)
            {
                
            }

            var chargeId =  ProcessPayment(model);
            return chargeId;
        }

        private string ProcessPayment(StripeChargeModel model)
        {
            var options = new ChargeCreateOptions
            {
                Amount = model.Amount,
                Currency = "sgd",
                Description = "Example charge",
                Source = model.Token,
            };
            var service = new ChargeService();
            Charge charge = service.Create(options);

            return charge.Id;
        }
    }
}
