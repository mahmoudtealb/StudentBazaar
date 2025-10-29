
namespace StudentBazaar.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // 💡 التعديل الرئيسي: بما أنك حذفت ErrorViewModel، نستخدم ViewData
            // لتمرير RequestId إلى View بدلاً من نموذج (Model)
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            // ببساطة نرجع View() وسيعرض ملف Views/Shared/Error.cshtml
            return View();
        }
    }
}