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
            CreateRecipeViewModel createViewModel = new CreateRecipeViewModel()
            {
                Categories = this.GetCategories()
            };
            return View(createViewModel);
        }

        // POST: Recipe/Create
        [HttpPost]
        public async Task<ActionResult> Create(CreateRecipeViewModel recipeViewModel, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                string imagePath = await ImageUtil.UploadImage(image, "Recipe");
                recipeViewModel.ImagePath = imagePath;
                this._recipeService.Add(this._mapper.Map<Recipe>(recipeViewModel));
                return RedirectToAction("Index", "Home");
            }
            recipeViewModel.Categories = this.GetCategories();
            return View(recipeViewModel);
        }

        // GET: Recipe/Details/id
        public ActionResult Details(long id)
        {
            List<Recipe> recipes = this._recipeService.GetByCategoryId(id);
            RecipeDetailsViewModel recipeDetailsViewModel = new RecipeDetailsViewModel();
            recipeDetailsViewModel.Recipes = this._mapper.Map<List<RecipeDetailsViewModel>>(recipes);
            return View(recipeDetailsViewModel);
        }

        // GET: Recipe/More/Id
        public ActionResult More(long id)
        {
            Recipe recipe = this._recipeService.GetById(id);
            MoreRecipeViewModel moreViewModel = this._mapper.Map<MoreRecipeViewModel>(recipe);
            return View(moreViewModel);
        }

        // GET: Recipe/Update/Id
        public ActionResult Update(long id)
        {
            Recipe recipe = this._recipeService.GetById(id);
            UpdateRecipeViewModel updateViewModel = this._mapper.Map<UpdateRecipeViewModel>(recipe);
            updateViewModel.Categories = this.GetCategories();
            return View(updateViewModel);
        }

        // POST: Recipe/Update/Id
        [HttpPost]
        public async Task<ActionResult> Update(long id, UpdateRecipeViewModel recipeViewModel, IFormFile image, String oldImagePath)
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
            recipeViewModel.Categories = this.GetCategories();
            return View(recipeViewModel);
        }

        // GET: Recipe/Delete/Id
        public ActionResult Delete(long id)
        {
            Recipe recipe = this._recipeService.GetById(id);
            recipe.Category = this._categoryService.GetById(recipe.CategoryId);
            DeleteRecipeViewModel deleteViewModel = this._mapper.Map<DeleteRecipeViewModel>(recipe);
            return View(deleteViewModel);
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
