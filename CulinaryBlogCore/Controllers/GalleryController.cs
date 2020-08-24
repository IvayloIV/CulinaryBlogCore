using Microsoft.AspNetCore.Mvc;

namespace CulinaryBlogCore.Controllers
{
    public class GalleryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
