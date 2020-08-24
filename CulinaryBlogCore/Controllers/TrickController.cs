using Microsoft.AspNetCore.Mvc;

namespace CulinaryBlogCore.Controllers
{
    public class TrickController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
