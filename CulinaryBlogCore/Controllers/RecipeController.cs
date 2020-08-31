using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CulinaryBlogCore.Data.Models.Entities;
using CulinaryBlogCore.Data.Models.Identity;
using CulinaryBlogCore.Enums;
using CulinaryBlogCore.Models.CategoryViewModels;
using CulinaryBlogCore.Models.RecipeViewModels;
using CulinaryBlogCore.Services.Contracts;

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Imgur.API.Endpoints;
using Imgur.API.Models;
using Microsoft.AspNetCore.Routing;

namespace CulinaryBlogCore.Controllers
{
    public class RecipeController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IRecipeService _recipeService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IImgurTokenService _imgurTokenService;

        public RecipeController(
                ICategoryService categoryService, 
                IRecipeService recipeService, 
                IMapper mapper,
                IImgurTokenService imgurTokenService,
                UserManager<ApplicationUser> userManager) {
            this._categoryService = categoryService;
            this._recipeService = recipeService;
            this._mapper = mapper;
            this._userManager = userManager;
            this._imgurTokenService = imgurTokenService;
        }

        [Authorize]
        [Route("[controller]/[action]")]
        [Route("Administration/[controller]/[action]")]
        public ActionResult Create()
        {
            CreateRecipeViewModel createViewModel = new CreateRecipeViewModel()
            {
                Categories = this.GetCategories()
            };
            return View(createViewModel);
        }

        [HttpPost]
        [Authorize]
        [Route("[controller]/[action]")]
        [Route("Administration/[controller]/[action]")]
        public async Task<ActionResult> Create(CreateRecipeViewModel recipeViewModel)
        {
            if (ModelState.IsValid)
            {
                ImageEndpoint imageEndpoint = await this._imgurTokenService.GetImageEndpoint();
                IImage imageData = await imageEndpoint.UploadImageAsync(recipeViewModel.Image.OpenReadStream());
                recipeViewModel.ImagePath = imageData.Link;
                recipeViewModel.ImageId = imageData.Id;

                ApplicationUser user = await this._userManager.GetUserAsync(HttpContext.User);
                recipeViewModel.UserId = user.Id;
                this._recipeService.Add(this._mapper.Map<Recipe>(recipeViewModel));
                return RedirectToAction("Index", "Home");
            }
            recipeViewModel.Categories = this.GetCategories();
            return View(recipeViewModel);
        }

        public async Task<ActionResult> Details(long id)
        {
            ApplicationUser user = await this._userManager.GetUserAsync(HttpContext.User);
            List<Recipe> recipes = this._recipeService.GetByCategoryId(id);
            RecipeDetailsViewModel recipeDetailsViewModel = this.CreateRecipeDetailsViewModel(user, recipes);
            recipeDetailsViewModel.Category = this._categoryService.GetById(id);
            return View(recipeDetailsViewModel);
        }

        public async Task<ActionResult> More(long id)
        {
            ApplicationUser user = await this._userManager.GetUserAsync(HttpContext.User);
            Recipe recipe = this._recipeService.UpdateViewCount(id);
            RecipeDetailsViewModel moreViewModel = this._mapper.Map<RecipeDetailsViewModel>(recipe);
            this.CalculateRatingSingle(recipe, moreViewModel);
            if (user != null)
            {
                moreViewModel.CurrUserId = user.Id;
                this.CalculateUserVotesSingle(moreViewModel, user, recipe);
            }
            return View(moreViewModel);
        }

        [Authorize]
        public async Task<ActionResult> Update(long id)
        {
            Recipe recipe = this._recipeService.GetById(id);
            UpdateRecipeViewModel updateViewModel = this._mapper.Map<UpdateRecipeViewModel>(recipe);

            if (await this.IsAdminOrOwner(updateViewModel.UserId)) {
                updateViewModel.Categories = this.GetCategories();
                return View(updateViewModel);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Update(long id, UpdateRecipeViewModel recipeViewModel)
        {
            if (ModelState.IsValid)
            {
                if (await this.IsAdminOrOwner(recipeViewModel.UserId))
                {
                    if (recipeViewModel.Image != null)
                    {
                        ImageEndpoint imageEndpoint = await this._imgurTokenService.GetImageEndpoint();
                        await imageEndpoint.DeleteImageAsync(recipeViewModel.ImageId);
                        IImage uploadedImage = await imageEndpoint.UploadImageAsync(recipeViewModel.Image.OpenReadStream());
                        recipeViewModel.ImageId = uploadedImage.Id;
                        recipeViewModel.ImagePath = uploadedImage.Link;
                    }

                    this._recipeService.UpdateById(id, this._mapper.Map<Recipe>(recipeViewModel));
                }
                return RedirectToAction("Index", "Home");
            }
            recipeViewModel.Id = id;
            recipeViewModel.Categories = this.GetCategories();
            return View(recipeViewModel);
        }

        [Authorize]
        public async Task<ActionResult> Delete(long id)
        {
            Recipe recipe = this._recipeService.GetById(id);
            if (await this.IsAdminOrOwner(recipe.UserId))
            {
                recipe.Category = this._categoryService.GetById(recipe.CategoryId);
                DeleteRecipeViewModel deleteViewModel = this._mapper.Map<DeleteRecipeViewModel>(recipe);
                return View(deleteViewModel);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Delete(long id, string imageId)
        {
            Recipe recipe = this._recipeService.GetById(id);
            if (await this.IsAdminOrOwner(recipe.UserId) && imageId != null)
            {
                ImageEndpoint imageEndpoint = await this._imgurTokenService.GetImageEndpoint();
                await imageEndpoint.DeleteImageAsync(imageId);
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

        [Authorize]
        public async Task<ActionResult> MyRecipes()
        {
            ApplicationUser user = await this._userManager.GetUserAsync(HttpContext.User);
            List<Recipe> recipes = this._recipeService.GetByUserId(user.Id);
            RecipeDetailsViewModel recipeDetailsViewModel = this.CreateRecipeDetailsViewModel(user, recipes);
            return View(recipeDetailsViewModel);
        }

        public IActionResult Gallery() {
            List<Recipe> recipes = this._recipeService.GetAll();
            List<CategoryViewModel> categories = this.GetCategories();
            GalleryViewModel galleryViewModel = new GalleryViewModel
            {
                Recipes = this._mapper.Map<List<RecipeViewModel>>(recipes),
                Categories = categories
            };

            return View(galleryViewModel);
        }

        private List<CategoryViewModel> GetCategories()
        {
            IList<Category> categories = this._categoryService.GetAll();
            return this._mapper.Map<List<CategoryViewModel>>(categories);
        }

        private async Task<bool> IsAdminOrOwner(string userId) {
            ApplicationUser user = await this._userManager.GetUserAsync(HttpContext.User);
            IList<string> roles = await _userManager.GetRolesAsync(user);

            return roles.Any(r => r == Role.Admin.ToString()) || user.Id == userId;
        }

        private void CalculateRating(List<Recipe> recipes, List<RecipeDetailsViewModel> recipeViewModel)
        {
            for (int i = 0; i < recipes.Count; i++)
            {
                Recipe currRecipe = recipes[i];
                RecipeDetailsViewModel currRecipeViewModel = recipeViewModel.Find(r => r.Id == currRecipe.Id);
                this.CalculateRatingSingle(currRecipe, currRecipeViewModel);
            }
        }

        private void CalculateUserVotes(List<RecipeDetailsViewModel> recipeViewModel, ApplicationUser user, IList<Recipe> recipes)
        {
            for (int i = 0; i < recipes.Count; i++)
            {
                Recipe currRecipe = recipes[i];
                RecipeDetailsViewModel currRecipeViewModel = recipeViewModel.Find(r => r.Id == currRecipe.Id);
                this.CalculateUserVotesSingle(currRecipeViewModel, user, currRecipe);
            }
        }

        private void CalculateRatingSingle(Recipe currRecipe, RecipeDetailsViewModel currRecipeViewModel) {
            double rating = currRecipe.UserRecipeRatings.Sum(ur => ur.Rating) / (double)Math.Max(currRecipe.UserRecipeRatings.Count, 1);
            currRecipeViewModel.Rating = $"{rating:F2}";
            currRecipeViewModel.VoteCount = currRecipe.UserRecipeRatings.Count;
        }

        private void CalculateUserVotesSingle(RecipeDetailsViewModel currRecipeViewModel, ApplicationUser user, Recipe currRecipe) {
            UserRecipeRating userRecipeRating = currRecipe.UserRecipeRatings.FirstOr(ur => ur.UserId == user.Id, null);
            currRecipeViewModel.UserRating = userRecipeRating != null ? userRecipeRating.Rating : 0;
        }

        private RecipeDetailsViewModel CreateRecipeDetailsViewModel(ApplicationUser user, List<Recipe> recipes) {
            RecipeDetailsViewModel recipeDetailsViewModel = new RecipeDetailsViewModel();
            recipeDetailsViewModel.Recipes = this._mapper.Map<List<RecipeDetailsViewModel>>(recipes);
            this.CalculateRating(recipes, recipeDetailsViewModel.Recipes);
            if (user != null)
            {
                recipeDetailsViewModel.CurrUserId = user.Id;
                this.CalculateUserVotes(recipeDetailsViewModel.Recipes, user, recipes);
            }

            return recipeDetailsViewModel;
        }
    }
}
