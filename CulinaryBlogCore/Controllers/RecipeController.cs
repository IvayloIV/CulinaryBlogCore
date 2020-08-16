using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CulinaryBlogCore.Data.Models.Entities;
using CulinaryBlogCore.Models;
using CulinaryBlogCore.Models.CategoryViewModels;
using CulinaryBlogCore.Models.RecipeViewModels;
using CulinaryBlogCore.Services.Contracts;
using CulinaryBlogCore.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CulinaryBlogCore.Controllers
{
    public class RecipeController : Controller
    {
        private ICategoryService _categoryService;
        private IRecipeService _recipeService;
        private readonly IMapper _mapper;

        public RecipeController(ICategoryService categoryService, IRecipeService recipeService, IMapper mapper) {
            this._categoryService = categoryService;
            this._recipeService = recipeService;
            this._mapper = mapper;
        }

        // GET: Recipe/Create
        public ActionResult Create()
        {
            ViewData["Categories"] = this.GetCategories();
            return View();
        }

        // POST: Recipe/Create
        [HttpPost]
        public async Task<ActionResult> Create(CreateViewModel recipeViewModel, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                string imagePath = await ImageUtil.UploadImage(image, "Recipe");
                recipeViewModel.ImagePath = imagePath;
                this._recipeService.Add(this._mapper.Map<Recipe>(recipeViewModel));
                return RedirectToAction("Index", "Home");
            }
            ViewData["Categories"] = this.GetCategories();
            return View(recipeViewModel);
        }

        // GET: Recipe/Details/Salad
        public ActionResult Details(string recipeType)
        {
            return View();
        }

        // GET: Recipe/More/Id
        public ActionResult More(long id)
        {
            Recipe recipe = this._recipeService.GetById(id);
            ViewData["RecipeMore"] = this._mapper.Map<MoreViewModel>(recipe);
            return View();
        }

        private List<CategoryViewModel> GetCategories()
        {
            IList<Category> categories = this._categoryService.GetAll();
            return this._mapper.Map<List<CategoryViewModel>>(categories);
        }
    }
}
