using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CulinaryBlogCore.Models;
using CulinaryBlogCore.Services.Contracts;
using AutoMapper;
using CulinaryBlogCore.Data.Models.Entities;
using CulinaryBlogCore.Models.RecipeViewModels;
using CulinaryBlogCore.Data.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Internal;

namespace CulinaryBlogCore.Controllers
{
    public class HomeController : Controller
    {
        private IRecipeService _recipeService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(
            IRecipeService recipeService, 
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            this._recipeService = recipeService;
            this._mapper = mapper;
            this._userManager = userManager;
        }

        public async Task<ActionResult> Index()
        {
            ApplicationUser user = await this._userManager.GetUserAsync(HttpContext.User);
            IList<Recipe> recipesByRating = this._recipeService.GetByRatingWeek();
            List<RecipeViewModel> recipesByRatingModel = this._mapper.Map<List<RecipeViewModel>>(recipesByRating);
            this.CalculateRating(recipesByRating, recipesByRatingModel, user);
            if (user != null) {
                this.CalculateUserVotes(recipesByRatingModel, user, recipesByRating);
            }

            IList<Recipe> lastAddedRecipes = this._recipeService.GetLastAdded();
            List<RecipeViewModel> lastAddedRecipesModel = this._mapper.Map<List<RecipeViewModel>>(lastAddedRecipes);

            RecipeHomeViewModel recipeHomeViewModel = new RecipeHomeViewModel() { 
                recipesByRating = recipesByRatingModel,
                lastAddedRecipes = lastAddedRecipesModel,
                CurrUserId = user?.Id
            };
            return View(recipeHomeViewModel);
        }

        private void CalculateRating(IList<Recipe> recipes, List<RecipeViewModel> recipeViewModel, ApplicationUser user) {
            for (int i = 0; i < recipes.Count; i++)
            {
                Recipe currRecipe = recipes[i];
                RecipeViewModel currRecipeViewModel = recipeViewModel.Find(r => r.Id == currRecipe.Id);
                double rating = currRecipe.UserRecipeRatings.Sum(ur => ur.Rating) / (double)Math.Max(currRecipe.UserRecipeRatings.Count, 1);
                currRecipeViewModel.Rating = $"{rating:F2}";
                currRecipeViewModel.VoteCount = currRecipe.UserRecipeRatings.Count;
            }
        }

        private void CalculateUserVotes(List<RecipeViewModel> recipeViewModel, ApplicationUser user, IList<Recipe> recipes) {
            for (int i = 0; i < recipes.Count; i++)
            {
                Recipe currRecipe = recipes[i];
                RecipeViewModel currRecipeViewModel = recipeViewModel.Find(r => r.Id == currRecipe.Id);
                UserRecipeRating userRecipeRating = currRecipe.UserRecipeRatings.FirstOr(ur => ur.UserId == user.Id, null);
                currRecipeViewModel.UserRating = userRecipeRating != null ? userRecipeRating.Rating : 0;
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
