using Microsoft.AspNetCore.Mvc;
using visualize.Models;

namespace visualize.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(HomeIndexViewModel vm)
        {
            return View(vm);
        }
    }
}
