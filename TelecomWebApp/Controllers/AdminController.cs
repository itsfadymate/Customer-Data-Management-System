using Microsoft.AspNetCore.Mvc;

namespace TelecomWebApp.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
