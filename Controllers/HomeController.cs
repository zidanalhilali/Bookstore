using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Error(int? code)
        {
            ViewBag.Code = code;
            return View();
        }
    }
}