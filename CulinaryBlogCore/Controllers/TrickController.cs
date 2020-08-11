using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
