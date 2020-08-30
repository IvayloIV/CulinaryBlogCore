using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using CulinaryBlogCore.Models;
using CulinaryBlogCore.Services.Contracts;
using CulinaryBlogCore.Data.Models.Entities;
using CulinaryBlogCore.Models.RecipeViewModels;
using CulinaryBlogCore.Data.Models.Identity;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using CulinaryBlogCore.Models.ChefViewModels;

namespace CulinaryBlogCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRecipeService _recipeService;
        private readonly IChefService _chefService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(
            IRecipeService recipeService,
            IChefService chefService, 
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            this._recipeService = recipeService;
            this._chefService = chefService;
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

            List<Chef> chefs = this._chefService.GetAll();
            List<ChefViewModel> chefsViewModel = this._mapper.Map<List<ChefViewModel>>(chefs);

            RecipeHomeViewModel recipeHomeViewModel = new RecipeHomeViewModel() { 
                RecipesByRating = recipesByRatingModel,
                LastAddedRecipes = lastAddedRecipesModel,
                Chefs = chefsViewModel,
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
