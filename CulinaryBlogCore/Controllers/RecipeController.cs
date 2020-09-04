using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CulinaryBlogCore.Data.Models.Entities;
using CulinaryBlogCore.Data.Models.Identity;
using CulinaryBlogCore.Models.CategoryViewModels;
using CulinaryBlogCore.Models.RecipeViewModels;
using CulinaryBlogCore.Services.Contracts;

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace CulinaryBlogCore.Controllers
{
    [Authorize]
    public class RecipeController : BaseController
    {
        private const double DefaultRating = 0;    

        private readonly ICategoryService _categoryService;
        private readonly IRecipeService _recipeService;
        private readonly IMapper _mapper;
        private readonly IImgurService _imgurService;

        public RecipeController(
            ICategoryService categoryService,
            IRecipeService recipeService,
            IMapper mapper,
            IImgurService imgurService,
            UserManager<ApplicationUser> userManager)
            : base(userManager)
        {
            this._categoryService = categoryService;
            this._recipeService = recipeService;
            this._mapper = mapper;
            this._imgurService = imgurService;
        }

        [Route("[controller]/[action]")]
        [Route("Administration/[controller]/[action]")]
        public ActionResult Create()
        {
            CreateRecipeViewModel model = new CreateRecipeViewModel()
            {
                Categories = this.GetCategories()
            };

            return View(model);
        }

        [HttpPost]
        [Route("[controller]/[action]")]
        [Route("Administration/[controller]/[action]")]
        public async Task<ActionResult> Create(CreateRecipeViewModel recipeViewModel)
        {
            if (ModelState.IsValid && recipeViewModel.Image != null)
            {
                await this._imgurService.UploadImage(recipeViewModel);

                ApplicationUser user = await base._userManager.GetUserAsync(HttpContext.User);
                recipeViewModel.UserId = user.Id;
                this._recipeService.Add(this._mapper.Map<Recipe>(recipeViewModel));

                return RedirectToAction("Index", "Home");
            }

            if (recipeViewModel.Image == null) 
            {
                ModelState.AddModelError("Image", "The Image field is required.");
            }

            recipeViewModel.Categories = this.GetCategories();
            return View(recipeViewModel);
        }

        [AllowAnonymous]
        public async Task<ActionResult> Details(long id)
        {
            ApplicationUser user = await base._userManager.GetUserAsync(HttpContext.User);
            List<Recipe> recipes = this._recipeService.GetByCategoryId(id);
            RecipeDetailsViewModel model = this.CreateRecipeDetailsViewModel(recipes, user);
            model.Category = this._categoryService.GetById(id);

            return View(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> More(long id)
        {
            ApplicationUser user = await base._userManager.GetUserAsync(HttpContext.User);
            Recipe recipe = this._recipeService.UpdateViewCount(id);
            this._recipeService.CalculateRecipeRating(recipe);

            if (user != null)
            {
                this._recipeService.CalculateUserVote(recipe, user.Id);
            }

            RecipeDetailsViewModel model = this._mapper.Map<RecipeDetailsViewModel>(recipe);

            if (user != null)
            {
                model.CurrentUserId = user.Id;
            }

            return View(model);
        }

        public async Task<ActionResult> Update(long id)
        {
            Recipe recipe = this._recipeService.GetById(id);
            UpdateRecipeViewModel model = this._mapper.Map<UpdateRecipeViewModel>(recipe);

            if (await base.IsAdminOrOwner(model.UserId))
            {
                model.Categories = this.GetCategories();

                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> Update(long id, UpdateRecipeViewModel recipeViewModel)
        {
            if (ModelState.IsValid)
            {
                if (await base.IsAdminOrOwner(recipeViewModel.UserId))
                {
                    if (recipeViewModel.Image != null)
                    {
                        await this._imgurService.EditImage(recipeViewModel);
                    }

                    this._recipeService.UpdateById(id, this._mapper.Map<Recipe>(recipeViewModel));
                }

                return RedirectToAction("Index", "Home");
            }

            recipeViewModel.Id = id;
            recipeViewModel.Categories = this.GetCategories();

            return View(recipeViewModel);
        }

        public async Task<ActionResult> Delete(long id)
        {
            Recipe recipe = this._recipeService.GetById(id);

            if (await base.IsAdminOrOwner(recipe.UserId))
            {
                DeleteRecipeViewModel deleteViewModel = this._mapper.Map<DeleteRecipeViewModel>(recipe);
                return View(deleteViewModel);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(long id, string imageId)
        {
            Recipe recipe = this._recipeService.GetById(id);

            if (await base.IsAdminOrOwner(recipe.UserId) && imageId != null)
            {
                await this._imgurService.DeleteImage(imageId);
                this._recipeService.DeleteById(id);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<double> ChangeRating(RecipeRatingViewModel recipeRatingViewModel)
        {
            ApplicationUser user = await base._userManager.GetUserAsync(HttpContext.User);
            user.UserRecipeRatings = this._recipeService.GetRecipeRatingByUser(user.Id);
            Recipe recipe = this._recipeService.GetById(recipeRatingViewModel.RecipeId);

            if (user.Id != recipe.UserId && (user.UserRecipeRatings == null || user.UserRecipeRatings.All(r => r.RecipeId != recipeRatingViewModel.RecipeId)))
            {
                recipeRatingViewModel.UserId = user.Id;
                UserRecipeRating userRecipeRating = this._mapper.Map<UserRecipeRating>(recipeRatingViewModel);
                recipe = this._recipeService.UpdateByRating(userRecipeRating);

                return this._recipeService.CalculateRating(recipe);
            }

            return DefaultRating;
        }

        public async Task<ActionResult> MyRecipes()
        {
            ApplicationUser user = await base._userManager.GetUserAsync(HttpContext.User);
            List<Recipe> recipes = this._recipeService.GetByUserId(user.Id);
            RecipeDetailsViewModel model = this.CreateRecipeDetailsViewModel(recipes, user);

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Gallery()
        {
            GalleryViewModel model = new GalleryViewModel();

            foreach (var gallery in this._categoryService.GetRecipesByCategory()) {
                List<Recipe> recipes = gallery.SelectMany(r => r).ToList();
                List<RecipeViewModel> recipesModel = this._mapper.Map<List<RecipeViewModel>>(recipes);
                model.Galleries.Add(new GalleryViewModel(gallery.Key, recipesModel));
            }

            return View(model);
        }

        private List<CategoryViewModel> GetCategories()
        {
            IList<Category> categories = this._categoryService.GetAll();

            return this._mapper.Map<List<CategoryViewModel>>(categories);
        }

        private RecipeDetailsViewModel CreateRecipeDetailsViewModel(List<Recipe> recipes, ApplicationUser user)
        {
            RecipeDetailsViewModel model = new RecipeDetailsViewModel();
            this._recipeService.CalculateRecipesRating(recipes);

            if (user != null)
            {
                model.CurrentUserId = user.Id;
                this._recipeService.CalculateUserVotes(recipes, user.Id);
            }

            model.Recipes = this._mapper.Map<List<RecipeDetailsViewModel>>(recipes);
            return model;
        }
    }
}
