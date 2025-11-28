using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using StudentBazaar.Web.Hubs;
using StudentBazaar.Web.Models;
using StudentBazaar.Web.Repositories;
using StudentBazaar.Web.ViewModels;

namespace StudentBazaar.Web.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IGenericRepository<ChatMessage> _chatRepo;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(
            IGenericRepository<ChatMessage> chatRepo,
            IGenericRepository<Product> productRepo,
            UserManager<ApplicationUser> userManager,
            IHubContext<ChatHub> hubContext)
        {
            _chatRepo = chatRepo;
            _productRepo = productRepo;
            _userManager = userManager;
            _hubContext = hubContext;
        }

        private int GetCurrentUserId()
        {
            var idStr = _userManager.GetUserId(User);
            return int.Parse(idStr!);
        }

        // مفتاح الجروب لنفس المحادثة بين نفس الشخصين على نفس المنتج
        private string BuildConversationKey(int user1, int user2, int productId)
        {
            var a = Math.Min(user1, user2);
            var b = Math.Max(user1, user2);
            return $"chat-{a}-{b}-p{productId}";
        }

        // ==========================
        // 1) فتح محادثة مع صاحب المنتج (من زرار Buy / Contact)
        // ==========================
        [HttpGet]
        public async Task<IActionResult> WithSeller(int productId)
        {
            var product = await _productRepo.GetFirstOrDefaultAsync(
                p => p.Id == productId,
                includeWord: "Owner");

            if (product == null)
                return NotFound();

            if (product.OwnerId == null)
                return BadRequest("Product has no owner.");

            var currentUserId = GetCurrentUserId();
            var otherUserId = product.OwnerId.Value;

            var messages = await _chatRepo.GetAllAsync(
                m => m.ProductId == productId &&
                     ((m.SenderId == currentUserId && m.ReceiverId == otherUserId) ||
                      (m.SenderId == otherUserId && m.ReceiverId == currentUserId)),
                includeWord: "Sender,Receiver");

            var vm = new ChatConversationViewModel
            {
                ProductId = product.Id,
                Product = product,
                OtherUserId = otherUserId,
                OtherUserName = product.Owner.FullName,
                Messages = messages.OrderBy(m => m.SentAt).ToList(),
                CurrentUserId = currentUserId,
                IsSeller = (currentUserId == product.OwnerId),
                ConversationKey = BuildConversationKey(currentUserId, otherUserId, product.Id)
            };

            return View("Conversation", vm);
        }

        // نفس شاشة المحادثة لكن تُفتح من صفحة "محادثاتك"
        [HttpGet]
        public async Task<IActionResult> Open(int productId, int otherUserId)
        {
            var product = await _productRepo.GetFirstOrDefaultAsync(
                p => p.Id == productId,
                includeWord: "Owner");

            if (product == null)
                return NotFound();

            var currentUserId = GetCurrentUserId();

            var messages = await _chatRepo.GetAllAsync(
                m => m.ProductId == productId &&
                     ((m.SenderId == currentUserId && m.ReceiverId == otherUserId) ||
                      (m.SenderId == otherUserId && m.ReceiverId == currentUserId)),
                includeWord: "Sender,Receiver");

            var otherUser = messages
                .Select(m => m.SenderId == currentUserId ? m.Receiver : m.Sender)
                .FirstOrDefault() ?? product.Owner;

            var vm = new ChatConversationViewModel
            {
                ProductId = product.Id,
                Product = product,
                OtherUserId = otherUserId,
                OtherUserName = otherUser?.FullName ?? "User",
                Messages = messages.OrderBy(m => m.SentAt).ToList(),
                CurrentUserId = currentUserId,
                IsSeller = (currentUserId == product.OwnerId),
                ConversationKey = BuildConversationKey(currentUserId, otherUserId, product.Id)
            };

            return View("Conversation", vm);
        }

        // ==========================
        // 2) إرسال رسالة  (AJAX + SignalR)
        // ==========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send([FromForm] ChatConversationViewModel model)
        {
            var senderId = GetCurrentUserId();

            if (string.IsNullOrWhiteSpace(model.NewMessage))
            {
                // بدل ما نرمي BadRequest في وش المستخدم، نرجع JSON
                return Json(new { success = false, error = "Message is required." });
            }

            var message = new ChatMessage
            {
                SenderId = senderId,
                ReceiverId = model.OtherUserId,
                ProductId = model.ProductId,
                Content = model.NewMessage,
                SentAt = DateTime.UtcNow
            };

            await _chatRepo.AddAsync(message);
            await _chatRepo.SaveAsync();

            var convKey = BuildConversationKey(senderId, model.OtherUserId, model.ProductId);

            await _hubContext.Clients.Group(convKey).SendAsync("ReceiveMessage", new
            {
                productId = model.ProductId,
                senderId = senderId,
                content = message.Content,
                sentAt = message.SentAt.ToLocalTime().ToString("yyyy/MM/dd - hh:mm tt")
            });

            return Json(new { success = true });
        }

        // ==========================
        // 3) صفحة "محادثاتك"
        // ==========================
        [HttpGet]
        public async Task<IActionResult> MyConversations()
        {
            var currentUserId = GetCurrentUserId();

            var allMessages = await _chatRepo.GetAllAsync(
                m => m.SenderId == currentUserId || m.ReceiverId == currentUserId,
                includeWord: "Product,Sender,Receiver");

            // كمشتري
            var asBuyer = allMessages
                .Where(m => m.Product!.OwnerId != currentUserId)
                .GroupBy(m => new
                {
                    m.ProductId,
                    OtherUserId = (m.SenderId == currentUserId ? m.ReceiverId : m.SenderId)
                })
                .Select(g =>
                {
                    var last = g.OrderByDescending(x => x.SentAt).First();
                    var other = last.SenderId == currentUserId ? last.Receiver : last.Sender;
                    var unread = g.Count(x => x.ReceiverId == currentUserId && !x.IsRead);

                    return new ChatConversationItemViewModel
                    {
                        ProductId = g.Key.ProductId ?? 0,
                        ProductName = last.Product?.Name ?? "",
                        OtherUserId = g.Key.OtherUserId,
                        OtherUserName = other?.FullName ?? "User",
                        LastMessage = last.Content,
                        LastMessageAt = last.SentAt,
                        UnreadCount = unread
                    };
                })
                .OrderByDescending(x => x.LastMessageAt)
                .ToList();

            // كبائع
            var asSeller = allMessages
                .Where(m => m.Product!.OwnerId == currentUserId)
                .GroupBy(m => new
                {
                    m.ProductId,
                    OtherUserId = (m.SenderId == currentUserId ? m.ReceiverId : m.SenderId)
                })
                .Select(g =>
                {
                    var last = g.OrderByDescending(x => x.SentAt).First();
                    var other = last.SenderId == currentUserId ? last.Receiver : last.Sender;
                    var unread = g.Count(x => x.ReceiverId == currentUserId && !x.IsRead);

                    return new ChatConversationItemViewModel
                    {
                        ProductId = g.Key.ProductId ?? 0,
                        ProductName = last.Product?.Name ?? "",
                        OtherUserId = g.Key.OtherUserId,
                        OtherUserName = other?.FullName ?? "User",
                        LastMessage = last.Content,
                        LastMessageAt = last.SentAt,
                        UnreadCount = unread
                    };
                })
                .OrderByDescending(x => x.LastMessageAt)
                .ToList();

            var vm = new ChatMyConversationsViewModel
            {
                AsBuyer = asBuyer,
                AsSeller = asSeller
            };

            return View(vm);
        }
    }
}
