using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CulinaryBlogCore.Data.Models.Entities;
using CulinaryBlogCore.Models;
using CulinaryBlogCore.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CulinaryBlogCore.Controllers
{
    public class RecipeController : Controller
    {
        private ICategoryService _categoryService;

        public RecipeController(ICategoryService categoryService) {
            this._categoryService = categoryService;
        }

        // GET: Recipe/Create
        public ActionResult Create()
        {
            IList<CategoryViewModel> categoryViewModels = new List<CategoryViewModel>();
            this._categoryService
                .getAll()
                .ForEach(c => categoryViewModels.Add(new CategoryViewModel(c.Id, c.Name)));

            ViewData["Categories"] = categoryViewModels;
            return View();
        }

        // POST: Recipe/Create
        [HttpPost]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Create));
            }
            catch
            {
                return View();
            }
        }

        // GET: RecipeController/Details/Salad
        public ActionResult Details(string recipeType)
        {
            return View();
        }
    }
}
