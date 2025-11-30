using Microsoft.AspNetCore.Mvc;
using StudentBazaar.Web.Models;

namespace StudentBazaar.Web.Controllers
{
    public class PaymentController : Controller
    {
        [HttpPost]
        public IActionResult Checkout()
        {
            // Stripe.net package is not installed, so redirect to Checkout page
            TempData["Error"] = "Credit card payment is currently unavailable. Please use PayPal or Vodafone Cash.";
            return RedirectToAction("Index", "Checkout");
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Cancel()
        {
            return View();
        }
    }
}
