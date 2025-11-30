public class DashboardController : Controller
{
    public ActionResult Index()
    {
        ViewBag.UsersCount = 120;
        ViewBag.ProductsCount = 45;
        ViewBag.OrdersCount = 30;
        return View();
    }
}
