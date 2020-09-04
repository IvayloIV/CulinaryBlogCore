using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using CulinaryBlogCore.Models;
using CulinaryBlogCore.Services.Contracts;
using CulinaryBlogCore.Data.Models.Entities;
using CulinaryBlogCore.Models.RecipeViewModels;
using CulinaryBlogCore.Data.Models.Identity;
using CulinaryBlogCore.Models.ChefViewModels;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

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
            List<Recipe> recipesByRating = this._recipeService.GetByRatingWeek();
            this._recipeService.CalculateRecipesRating(recipesByRating);

            if (user != null)
            {
                this._recipeService.CalculateUserVotes(recipesByRating, user.Id);
            }

            List<RecipeViewModel> recipesByRatingModel = this._mapper.Map<List<RecipeViewModel>>(recipesByRating);
            List<Recipe> lastAddedRecipes = this._recipeService.GetLastAdded();
            List<RecipeViewModel> lastAddedRecipesModel = this._mapper.Map<List<RecipeViewModel>>(lastAddedRecipes);

            List<Chef> chefs = this._chefService.GetAll();
            List<ChefViewModel> chefsViewModel = this._mapper.Map<List<ChefViewModel>>(chefs);

            RecipeHomeViewModel model = new RecipeHomeViewModel()
            {
                RecipesByRating = recipesByRatingModel,
                LastAddedRecipes = lastAddedRecipesModel,
                Chefs = chefsViewModel,
                CurrentUserId = user?.Id
            };

            return View(model);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
