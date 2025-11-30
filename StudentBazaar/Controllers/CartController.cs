using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentBazaar.Web.Models;
using StudentBazaar.Web.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace StudentBazaar.Web.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IShoppingCartItemRepository _cartRepo;
        private readonly IGenericRepository<Listing> _listingRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(
            IShoppingCartItemRepository cartRepo,
            IGenericRepository<Listing> listingRepo,
            UserManager<ApplicationUser> userManager)
        {
            _cartRepo = cartRepo;
            _listingRepo = listingRepo;
            _userManager = userManager;
        }

        // =======================
        // Get current user ID
        // =======================
        private int GetCurrentUserId()
        {
            var userIdStr = _userManager.GetUserId(User);
            return int.Parse(userIdStr!);
        }

        // =======================
        // View Cart
        // =======================
        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();
            var items = await _cartRepo.GetAllAsync(c => c.UserId == userId, includeWord: "Listing,Listing.Product,Listing.Product.Images");
            var userItems = items.ToList();
            return View(userItems);
        }

        // =======================
        // Add to Cart
        // =======================
        [HttpPost]
        public async Task<IActionResult> AddToCart(int listingId, int quantity = 1)
        {
            var userId = GetCurrentUserId();
            var listing = await _listingRepo.GetFirstOrDefaultAsync(l => l.Id == listingId);
            if (listing == null) return NotFound();

            var existing = await _cartRepo.GetFirstOrDefaultAsync(c => c.UserId == userId && c.ListingId == listingId);
            if (existing != null)
            {
                existing.Quantity += quantity;
                _cartRepo.Update(existing);
            }
            else
            {
                var cartItem = new ShoppingCartItem
                {
                    UserId = userId,
                    ListingId = listingId,
                    Quantity = quantity
                };
                await _cartRepo.AddAsync(cartItem);
            }

            await _cartRepo.SaveAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToCart(int productId, int quantity = 1)
        {
            var userId = GetCurrentUserId();
            var listing = await _listingRepo.GetFirstOrDefaultAsync(l => l.ProductId == productId);
            if (listing == null)
            {
                TempData["Error"] = "This product is not available for purchase.";
                return RedirectToAction("Details", "Product", new { id = productId });
            }

            return await AddToCart(listing.Id, quantity);
        }

        // =======================
        // Remove from Cart
        // =======================
        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            var item = await _cartRepo.GetFirstOrDefaultAsync(c => c.Id == id);
            if (item == null) return NotFound();
            if (item.UserId != GetCurrentUserId()) return Forbid();

            _cartRepo.Remove(item);
            await _cartRepo.SaveAsync();
            return RedirectToAction("Index");
        }

        // =======================
        // Checkout page
        // =======================
        public async Task<IActionResult> Checkout()
        {
            var userId = GetCurrentUserId();
            var items = await _cartRepo.GetAllAsync(c => c.UserId == userId, includeWord: "Listing,Listing.Product");
            var userItems = items.ToList();

            if (!userItems.Any())
            {
                TempData["Error"] = "Your cart is empty!";
                return RedirectToAction("Index");
            }

            var total = userItems.Sum(i => i.Listing.Price * i.Quantity);
            ViewBag.Total = total;
            return RedirectToAction("Index", "Checkout");
        }

        // =======================
        // Confirm Checkout
        // =======================
        [HttpPost]
        public async Task<IActionResult> ConfirmCheckout()
        {
            var userId = GetCurrentUserId();
            var items = await _cartRepo.GetAllAsync(c => c.UserId == userId);
            _cartRepo.RemoveRange(items);
            await _cartRepo.SaveAsync();
            TempData["Success"] = "Checkout completed successfully!";
            return RedirectToAction("Index");
        }
    }
}
