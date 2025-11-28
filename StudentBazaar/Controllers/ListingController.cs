using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using StudentBazaar.Web.Models;
using StudentBazaar.Web.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StudentBazaar.Web.Controllers
{
    [Authorize]
    public class ListingController : Controller
    {
        private readonly IGenericRepository<Listing> _listingRepo;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public ListingController(
            IGenericRepository<Listing> listingRepo,
            IGenericRepository<Product> productRepo,
            UserManager<ApplicationUser> userManager)
        {
            _listingRepo = listingRepo;
            _productRepo = productRepo;
            _userManager = userManager;
        }

        private int GetCurrentUserId()
        {
            var idStr = _userManager.GetUserId(User);
            return int.Parse(idStr!);
        }

        private bool CanManage(Listing listing)
        {
            if (User.IsInRole("Admin")) return true;
            if (!User.Identity!.IsAuthenticated) return false;
            return listing.SellerId == GetCurrentUserId();
        }

        // =======================
        // INDEX
        // =======================
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var listings = await _listingRepo.GetAllAsync(
                includeWord: "Product,Seller");

            return View(listings);
        }

        // =======================
        // DETAILS
        // =======================
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var listing = await _listingRepo.GetFirstOrDefaultAsync(
                l => l.Id == id,
                includeWord: "Product,Seller");

            if (listing == null)
                return NotFound();

            return View(listing);
        }

        // =======================
        // CREATE (GET)
        // =======================
        public async Task<IActionResult> Create()
        {
            await FillProductsDropDown();
            return View(new Listing());
        }

        // =======================
        // CREATE (POST)
        // =======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Listing entity)
        {
            if (!ModelState.IsValid)
            {
                await FillProductsDropDown();
                return View(entity);
            }

            entity.SellerId = GetCurrentUserId();
            entity.PostingDate = DateTime.UtcNow;

            await _listingRepo.AddAsync(entity);
            await _listingRepo.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        // =======================
        // EDIT (GET)
        // =======================
        public async Task<IActionResult> Edit(int id)
        {
            var listing = await _listingRepo.GetFirstOrDefaultAsync(
                l => l.Id == id);

            if (listing == null)
                return NotFound();

            if (!CanManage(listing))
                return Forbid();

            await FillProductsDropDown(listing.ProductId);

            return View(listing);
        }

        // =======================
        // EDIT (POST)
        // =======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Listing entity)
        {
            if (!ModelState.IsValid)
            {
                await FillProductsDropDown(entity.ProductId);
                return View(entity);
            }

            var existing = await _listingRepo.GetFirstOrDefaultAsync(
                l => l.Id == id);

            if (existing == null)
                return NotFound();

            if (!CanManage(existing))
                return Forbid();

            existing.Price = entity.Price;
            existing.Condition = entity.Condition;
            existing.Description = entity.Description;
            existing.Discount = entity.Discount;
            existing.Status = entity.Status;
            existing.ProductId = entity.ProductId;
            existing.UpdatedAt = DateTime.UtcNow;

            await _listingRepo.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        // =======================
        // DELETE (GET)
        // =======================
        public async Task<IActionResult> Delete(int id)
        {
            var listing = await _listingRepo.GetFirstOrDefaultAsync(
                l => l.Id == id,
                includeWord: "Product,Seller");

            if (listing == null)
                return NotFound();

            if (!CanManage(listing))
                return Forbid();

            return View(listing);
        }

        // =======================
        // DELETE (POST)
        // =======================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var listing = await _listingRepo.GetFirstOrDefaultAsync(l => l.Id == id);

            if (listing == null)
                return NotFound();

            if (!CanManage(listing))
                return Forbid();

            _listingRepo.Remove(listing);
            await _listingRepo.SaveAsync();

            return RedirectToAction(nameof(Index));
        }

        // =======================
        // Helper: Products Dropdown
        // =======================
        private async Task FillProductsDropDown(int? selectedId = null)
        {
            var products = await _productRepo.GetAllAsync();

            ViewBag.ProductList = products.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString(),
                Selected = (selectedId.HasValue && selectedId.Value == p.Id)
            }).ToList();
        }
    }
}
