using Microsoft.AspNetCore.Mvc;
using StudentBazaar.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace StudentBazaar.Web.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CheckoutController> _logger;

        public CheckoutController(IConfiguration configuration, ILogger<CheckoutController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        // GET: Checkout page
        [HttpGet]
        public IActionResult Index()
        {
            // مثال بيانات وهمية للسلة، لاحقًا ممكن تجي من قاعدة البيانات
            var cartItems = new List<CartItemViewModel>
            {
                new CartItemViewModel { ProductId = 1, ProductName = "Notebook", Quantity = 2, Price = 5 },
                new CartItemViewModel { ProductId = 2, ProductName = "Pen Set", Quantity = 1, Price = 10 }
            };

            var subtotal = cartItems.Sum(x => x.Price * x.Quantity);
            var model = new CheckoutViewModel
            {
                CartItems = cartItems,
                Subtotal = subtotal,
                Total = subtotal // هنا ممكن تضيف ضريبة أو شحن
            };

            // Check if Stripe is configured
            var stripeKey = _configuration["Stripe:SecretKey"];
            ViewBag.StripeConfigured = IsValidStripeKey(stripeKey);

            return View(model);
        }

        [HttpPost]
        public IActionResult CompletePayment(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            if (model.SelectedPaymentMethod == "Card")
            {
                // Stripe.net package is not installed, so card payment is unavailable
                _logger.LogWarning("Card payment selected but Stripe.net package is not installed.");
                TempData["Error"] = "Credit card payment is currently unavailable. Please use PayPal or Vodafone Cash.";
                return RedirectToAction("Index");
            }
            else if (model.SelectedPaymentMethod == "PayPal")
            {
                // Redirect to PayPal checkout
                var domain = $"{Request.Scheme}://{Request.Host}";
                var totalAmount = model.Total;
                var businessEmail = _configuration["PayPal:BusinessEmail"] ?? "sb-merchant@business.example.com"; // PayPal sandbox email
                var returnUrl = $"{domain}/Checkout/Success?paymentMethod=PayPal";
                var cancelUrl = $"{domain}/Checkout/Cancel?paymentMethod=PayPal";
                
                // Build PayPal checkout URL
                var paypalUrl = $"https://www.sandbox.paypal.com/checkoutnow?token=PAYMENT_TOKEN&amount={totalAmount:F2}&currency=USD&return={Uri.EscapeDataString(returnUrl)}&cancel_return={Uri.EscapeDataString(cancelUrl)}";
                
                // For production, you would create a PayPal order via API first
                // This is a simplified redirect approach
                // Redirect to a PayPal payment page
                TempData["PayPalAmount"] = totalAmount.ToString("F2");
                TempData["PayPalReturnUrl"] = returnUrl;
                TempData["PayPalCancelUrl"] = cancelUrl;
                return RedirectToAction("PayPalPayment");
            }
            else if (model.SelectedPaymentMethod == "VodafoneCash")
            {
                // Redirect to Vodafone Cash payment page
                TempData["VodafoneCashAmount"] = model.Total.ToString("F2");
                TempData["VodafoneCashItems"] = string.Join(", ", model.CartItems.Select(x => $"{x.ProductName} (x{x.Quantity})"));
                return RedirectToAction("VodafoneCashPayment");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Success(string paymentMethod = null)
        {
            ViewBag.Message = "Payment Successful 🎉";
            ViewBag.PaymentMethod = paymentMethod ?? "Unknown";
            return View();
        }

        [HttpGet]
        public IActionResult Cancel()
        {
            ViewBag.Message = "Payment Cancelled ❌";
            return View();
        }

        [HttpGet]
        public IActionResult PayPalPayment()
        {
            var amount = TempData["PayPalAmount"]?.ToString() ?? "0.00";
            var returnUrl = TempData["PayPalReturnUrl"]?.ToString() ?? $"{Request.Scheme}://{Request.Host}/Checkout/Success";
            var cancelUrl = TempData["PayPalCancelUrl"]?.ToString() ?? $"{Request.Scheme}://{Request.Host}/Checkout/Cancel";

            ViewBag.Amount = amount;
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.CancelUrl = cancelUrl;
            ViewBag.BusinessEmail = _configuration["PayPal:BusinessEmail"] ?? "sb-merchant@business.example.com";

            return View();
        }

        [HttpGet]
        public IActionResult VodafoneCashPayment()
        {
            var amount = TempData["VodafoneCashAmount"]?.ToString() ?? "0.00";
            var items = TempData["VodafoneCashItems"]?.ToString() ?? "";

            ViewBag.Amount = amount;
            ViewBag.Items = items;

            return View();
        }

        [HttpPost]
        public IActionResult CompleteVodafoneCashPayment(string phoneNumber)
        {
            // Simulate payment processing
            // In production, this would integrate with Vodafone Cash API
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                TempData["Error"] = "Please enter your Vodafone Cash phone number.";
                return RedirectToAction("VodafoneCashPayment");
            }

            // Here you would call Vodafone Cash API to process payment
            // For now, simulate successful payment
            _logger.LogInformation("Vodafone Cash payment initiated for phone: {PhoneNumber}", phoneNumber);
            
            return RedirectToAction("Success", new { paymentMethod = "VodafoneCash" });
        }

        private bool IsValidStripeKey(string? key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return false;

            // Check if it's a placeholder
            if (key.Contains("your_real") || 
                key.Contains("placeholder") || 
                key.Contains("replace") ||
                key.EndsWith("_here"))
                return false;

            // Check if it starts with sk_ (secret key) or pk_ (publishable key)
            // For API operations, we need sk_
            if (!key.StartsWith("sk_"))
                return false;

            // Check minimum length (Stripe keys are typically 32+ characters after the prefix)
            if (key.Length < 20)
                return false;

            return true;
        }
    }
}
