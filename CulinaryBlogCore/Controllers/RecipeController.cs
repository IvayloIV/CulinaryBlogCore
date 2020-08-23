using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CulinaryBlogCore.Data.Models.Entities;
using CulinaryBlogCore.Data.Models.Identity;
using CulinaryBlogCore.Enums;
using CulinaryBlogCore.Models.CategoryViewModels;
using CulinaryBlogCore.Models.RecipeViewModels;
using CulinaryBlogCore.Services.Contracts;
using CulinaryBlogCore.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CulinaryBlogCore.Controllers
{
    public class RecipeController : Controller
    {
        private ICategoryService _categoryService;
        private IRecipeService _recipeService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public RecipeController(
                ICategoryService categoryService, 
                IRecipeService recipeService, 
                IMapper mapper,
                UserManager<ApplicationUser> userManager) {
            this._categoryService = categoryService;
            this._recipeService = recipeService;
            this._mapper = mapper;
            this._userManager = userManager;
        }

        // GET: Recipe/Create
        //[Route("Administration/[controller]/[action]")]
        [Authorize]
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
        [Authorize]
        public async Task<ActionResult> Create(CreateRecipeViewModel recipeViewModel, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                string imagePath = await ImageUtil.UploadImage(image, "Recipe");
                recipeViewModel.ImagePath = imagePath;
                ApplicationUser user = await this._userManager.GetUserAsync(HttpContext.User);
                recipeViewModel.UserId = user.Id;
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
            recipeDetailsViewModel.Category = this._categoryService.GetById(id);
            recipeDetailsViewModel.Recipes = this._mapper.Map<List<RecipeDetailsViewModel>>(recipes);
            return View(recipeDetailsViewModel);
        }

        // GET: Recipe/More/Id
        public ActionResult More(long id)
        {
            Recipe recipe = this._recipeService.UpdateViewCount(id);
            MoreRecipeViewModel moreViewModel = this._mapper.Map<MoreRecipeViewModel>(recipe);
            return View(moreViewModel);
        }

        // GET: Recipe/Update/Id
        [Authorize]
        public async Task<ActionResult> Update(long id)
        {
            Recipe recipe = this._recipeService.GetById(id);
            UpdateRecipeViewModel updateViewModel = this._mapper.Map<UpdateRecipeViewModel>(recipe);

            if (await this.IsAdminOrOwner(() => updateViewModel.UserId)) {
                updateViewModel.Categories = this.GetCategories();
                return View(updateViewModel);
            }

            return RedirectToAction("Index", "Home");
        }

        // POST: Recipe/Update/Id
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Update(long id, UpdateRecipeViewModel recipeViewModel, IFormFile image, String oldImagePath)
        {
            if (ModelState.IsValid)
            {
                if (await this.IsAdminOrOwner(() => recipeViewModel.UserId))
                {
                    if (image != null)
                    {
                        ImageUtil.DeleteImage(oldImagePath);
                        string imagePath = await ImageUtil.UploadImage(image, "Recipe");
                        recipeViewModel.ImagePath = imagePath;
                    }
                    else
                    {
                        recipeViewModel.ImagePath = oldImagePath;
                    }
                    this._recipeService.UpdateById(id, this._mapper.Map<Recipe>(recipeViewModel));
                }
                return RedirectToAction("Index", "Home");
            }
            recipeViewModel.Id = id;
            recipeViewModel.ImagePath = oldImagePath;
            recipeViewModel.Categories = this.GetCategories();
            return View(recipeViewModel);
        }

        // GET: Recipe/Delete/Id
        [Authorize]
        public async Task<ActionResult> Delete(long id)
        {
            Recipe recipe = this._recipeService.GetById(id);
            if (await this.IsAdminOrOwner(() => recipe.UserId))
            {
                recipe.Category = this._categoryService.GetById(recipe.CategoryId);
                DeleteRecipeViewModel deleteViewModel = this._mapper.Map<DeleteRecipeViewModel>(recipe);
                return View(deleteViewModel);
            }

            return RedirectToAction("Index", "Home");
        }

        // POST: Recipe/Delete/Id
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Delete(long id, string imagePath)
        {
            Recipe recipe = this._recipeService.GetById(id);
            if (await this.IsAdminOrOwner(() => recipe.UserId) && imagePath != null && imagePath.Length > 1)
            {
                ImageUtil.DeleteImage(imagePath);
                this._recipeService.DeleteById(id);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        public async Task<double> ChangeRating(RecipeRatingViewModel recipeRatingViewModel)
        {
            ApplicationUser user = await this._userManager.GetUserAsync(HttpContext.User);
            user.UserRecipeRatings = this._recipeService.GetRecipeRatingByUser(user.Id);
            Recipe recipe = this._recipeService.GetById(recipeRatingViewModel.RecipeId);
            if (user.Id != recipe.UserId && (user.UserRecipeRatings == null || user.UserRecipeRatings.All(r => r.RecipeId != recipeRatingViewModel.RecipeId))) {
                recipeRatingViewModel.UserId = user.Id;
                UserRecipeRating userRecipeRating = this._mapper.Map<UserRecipeRating>(recipeRatingViewModel);
                recipe = this._recipeService.UpdateByRating(userRecipeRating);

                return recipe.UserRecipeRatings.Sum(r => r.Rating) / (double)Math.Max(recipe.UserRecipeRatings.Count, 1);
            }

            return 0;
        }

        private List<CategoryViewModel> GetCategories()
        {
            IList<Category> categories = this._categoryService.GetAll();
            return this._mapper.Map<List<CategoryViewModel>>(categories);
        }

        private async Task<bool> IsAdminOrOwner(Func<string> getUserId) {
            ApplicationUser user = await this._userManager.GetUserAsync(HttpContext.User);
            IList<string> roles = await _userManager.GetRolesAsync(user);

            return roles.Any(r => r == Role.Admin.ToString()) || user.Id == getUserId.Invoke();
        }
    }
}
