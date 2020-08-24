using Microsoft.AspNetCore.Mvc;

namespace CulinaryBlogCore.Controllers
{
    public class AdministrationController : Controller
    {
        [ActionName("Create")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
