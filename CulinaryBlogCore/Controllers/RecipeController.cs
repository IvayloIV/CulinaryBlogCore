using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CulinaryBlogCore.Controllers
{
    public class RecipeController : Controller
    {
        // GET: RecipeController/Details/Salad
        public ActionResult Details(string recipeType)
        {
            return View();
        }
    }
}
