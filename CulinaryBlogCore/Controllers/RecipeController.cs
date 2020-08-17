using System;
using System.Collections.Generic;
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

        // GET: Recipe/Update/Id
        public ActionResult Update(long id)
        {
            Recipe recipe = this._recipeService.GetById(id);
            ViewData["Categories"] = this.GetCategories();
            ViewData["RecipeUpdate"] = this._mapper.Map<UpdateViewModel>(recipe);
            return View();
        }

        // POST: Recipe/Update/Id
        [HttpPost]
        public async Task<ActionResult> Update(long id, UpdateViewModel recipeViewModel, IFormFile image, String oldImagePath)
        {
            if (ModelState.IsValid)
            {
                if (image != null) {
                    ImageUtil.DeleteImage(oldImagePath);
                    string imagePath = await ImageUtil.UploadImage(image, "Recipe");
                    recipeViewModel.ImagePath = imagePath;
                } else {
                    recipeViewModel.ImagePath = oldImagePath;
                }
                
                this._recipeService.UpdateById(id, this._mapper.Map<Recipe>(recipeViewModel));
                return RedirectToAction("Index", "Home");
            }
            recipeViewModel.Id = id;
            recipeViewModel.ImagePath = oldImagePath;
            ViewData["RecipeUpdate"] = recipeViewModel;
            ViewData["Categories"] = this.GetCategories();
            return View(recipeViewModel);
        }

        // GET: Recipe/Delete/Id
        public ActionResult Delete(long id)
        {
            Recipe recipe = this._recipeService.GetById(id);
            recipe.Category = this._categoryService.GetById(recipe.CategoryId);
            ViewData["RecipeDeleteModel"] = this._mapper.Map<DeleteViewModel>(recipe);
            return View();
        }

        // POST: Recipe/Delete/Id
        [HttpPost]
        public ActionResult Delete(long id, string imagePath)
        {
            if (imagePath != null && imagePath.Length > 1) {
                ImageUtil.DeleteImage(imagePath);
                this._recipeService.DeleteById(id);
            }
            return RedirectToAction("Index", "Home");
        }

        private List<CategoryViewModel> GetCategories()
        {
            IList<Category> categories = this._categoryService.GetAll();
            return this._mapper.Map<List<CategoryViewModel>>(categories);
        }
    }
}
