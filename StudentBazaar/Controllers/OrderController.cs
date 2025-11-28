using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentBazaar.Web.Models;
using StudentBazaar.Web.Repositories;

namespace StudentBazaar.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IGenericRepository<Order> _orderRepo;
        private readonly IGenericRepository<Listing> _listingRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(
            IGenericRepository<Order> orderRepo,
            IGenericRepository<Listing> listingRepo,
            UserManager<ApplicationUser> userManager)
        {
            _orderRepo = orderRepo;
            _listingRepo = listingRepo;
            _userManager = userManager;
        }

        private int GetCurrentUserId()
        {
            var idStr = _userManager.GetUserId(User);
            return int.Parse(idStr!);
        }

        // ===============================
        // 1) My Orders / All Orders
        // ===============================
        public async Task<IActionResult> Index()
        {
            IEnumerable<Order> orders;

            if (User.IsInRole("Admin"))
            {
                // «·«œ„‰ Ì‘Ê› ﬂ· «·√Ê—œ—“
                orders = await _orderRepo.GetAllAsync(includeWord: "Listing,Listing.Product,Buyer,Shipment");
            }
            else
            {
                // «·ÿ«·» Ì‘Ê› √Ê—œ—« Â »”
                var currentUserId = GetCurrentUserId();
                orders = await _orderRepo.GetAllAsync(
                    o => o.BuyerId == currentUserId,
                    includeWord: "Listing,Listing.Product,Buyer,Shipment"
                );
            }

            return View(orders);
        }

        // ===============================
        // 2) Details
        // ===============================
        public async Task<IActionResult> Details(int id)
        {
            var entity = await _orderRepo.GetFirstOrDefaultAsync(
                o => o.Id == id,
                includeWord: "Listing,Listing.Product,Buyer,Shipment"
            );

            if (entity == null)
                return NotFound();

            // «·ÿ«·» „« Ì‘Ê›‘ √Ê—œ— Õœ  «‰Ì
            if (!User.IsInRole("Admin") && entity.BuyerId != GetCurrentUserId())
                return Forbid();

            return View(entity);
            // ·Ê ·”Â „«⁄‰œﬂ‘ View ··‹ Details „„ﬂ‰  ⁄„· Ê«Õœ »”Ìÿ ·«Õﬁ«
        }

        // ===============================
        // 3) Create (Admin ›ﬁÿ - ÌœÊÌ)
        // ===============================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            // DropDown ··‹ Listings
            var listings = await _listingRepo.GetAllAsync(includeWord: "Product,Seller");
            ViewBag.ListingList = listings
                .Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = $"{l.Product?.Name ?? "Product"} - {l.Price:0.00}"
                })
                .ToList();

            // DropDown ··„” Œœ„Ì‰ („„ﬂ‰  ﬂ ›Ì »ﬂ· «·„” Œœ„Ì‰)
            var users = _userManager.Users.ToList();
            ViewBag.BuyerList = users
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.FullName
                })
                .ToList();

            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order entity)
        {
            if (!ModelState.IsValid)
            {
                // ·«“„ ‰—Ã⁄ ‰„·Ì «·‹ ViewBags  «‰Ì
                await FillDropDownsForCreateEdit(entity.ListingId, entity.BuyerId);
                return View(entity);
            }

            await _orderRepo.AddAsync(entity);
            await _orderRepo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        // ===============================
        // 4) Edit (Admin)
        // ===============================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var existing = await _orderRepo.GetFirstOrDefaultAsync(o => o.Id == id);
            if (existing == null)
                return NotFound();

            await FillDropDownsForCreateEdit(existing.ListingId, existing.BuyerId);
            return View(existing);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Order entity)
        {
            if (!ModelState.IsValid)
            {
                await FillDropDownsForCreateEdit(entity.ListingId, entity.BuyerId);
                return View(entity);
            }

            var existing = await _orderRepo.GetFirstOrDefaultAsync(o => o.Id == id);
            if (existing == null)
                return NotFound();

            existing.ListingId = entity.ListingId;
            existing.BuyerId = entity.BuyerId;
            existing.OrderDate = entity.OrderDate;
            existing.Status = entity.Status;
            existing.PaymentMethod = entity.PaymentMethod;
            existing.TotalAmount = entity.TotalAmount;
            existing.SiteCommission = entity.SiteCommission;
            existing.UpdatedAt = DateTime.Now;

            await _orderRepo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        // ===============================
        // 5) Delete (Admin)
        // ===============================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _orderRepo.GetFirstOrDefaultAsync(o => o.Id == id,
                includeWord: "Listing,Buyer");
            if (entity == null)
                return NotFound();

            return View(entity);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entity = await _orderRepo.GetFirstOrDefaultAsync(o => o.Id == id);
            if (entity == null)
                return NotFound();

            _orderRepo.Remove(entity);
            await _orderRepo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        // ===============================
        // 6) “—«— Buy „‰ «·ÿ«·»
        // ===============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Buy(int listingId)
        {
            var listing = await _listingRepo.GetFirstOrDefaultAsync(
                l => l.Id == listingId,
                includeWord: "Product"
            );

            if (listing == null)
                return NotFound();

            var currentUserId = GetCurrentUserId();

            // ·Ê ⁄«Ì“  „‰⁄ ’«Õ» «·„‰ Ã Ì‘ —Ì „‰ ‰›”Â:
            if (listing.SellerId == currentUserId)
                return BadRequest("You cannot buy your own listing.");

            var total = listing.Price;
            var commission = Math.Round(total * 0.05m, 2);   // 5% „À·«

            var order = new Order
            {
                ListingId = listing.Id,
                BuyerId = currentUserId,
                OrderDate = DateTime.Now,
                Status = Models.OrderStatus.Pending,
                PaymentMethod = Models.PaymentMethod.CashOnDelivery,
                TotalAmount = total,
                SiteCommission = commission,
                CreatedAt = DateTime.Now
            };

            await _orderRepo.AddAsync(order);
            await _orderRepo.SaveAsync();

            //  ﬁœ— Â‰«  ⁄œ¯· Õ«·… «·≈⁄·«‰ ·Ê Õ«»» (Sold „À·«)
            // listing.Status = ListingStatus.Sold;
            // await _listingRepo.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        // ===============================
        // Helper ·„·¡ «·‹ DropDowns
        // ===============================
        private async Task FillDropDownsForCreateEdit(int? selectedListingId = null, int? selectedBuyerId = null)
        {
            var listings = await _listingRepo.GetAllAsync(includeWord: "Product,Seller");
            ViewBag.ListingList = listings
                .Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = $"{l.Product?.Name ?? "Product"} - {l.Price:0.00}",
                    Selected = (selectedListingId.HasValue && l.Id == selectedListingId.Value)
                })
                .ToList();

            var users = _userManager.Users.ToList();
            ViewBag.BuyerList = users
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.FullName,
                    Selected = (selectedBuyerId.HasValue && u.Id == selectedBuyerId.Value)
                })
                .ToList();
        }
    }
}
