using Microsoft.AspNetCore.Mvc;

namespace EventBookingSystem.API.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
