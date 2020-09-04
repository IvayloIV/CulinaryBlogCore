using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using CulinaryBlogCore.Data.Models.Entities;
using CulinaryBlogCore.Data.Models.Identity;
using CulinaryBlogCore.Services.Contracts;
using CulinaryBlogCore.Models.ResponseViewModel;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CulinaryBlogCore.Controllers
{
    public class MailController : Controller
    {
        private readonly IMailService _mailService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRecipeService _recipeService;

        public MailController(
            IMailService mailService,
            UserManager<ApplicationUser> userManager,
            IRecipeService recipeService)
        {
            this._mailService = mailService;
            this._userManager = userManager;
            this._recipeService = recipeService;
        }

        [HttpPost]
        [Route("[controller]/Recipe/Subscribe")]
        public async Task<JsonResult> SubscribeRecipes(string email)
        {
            ApplicationUser user = await this._userManager.GetUserAsync(HttpContext.User);
            RecipeSubscription recipeSubscription = new RecipeSubscription
            {
                Email = email,
                CreationTime = DateTime.Now,
                UserId = user?.Id
            };

            if (email == null || email.Length == 0)
            {
                return Json(new JsonViewModel(false, "Email cannot be empty!"));
            }

            if (this._mailService.CheckIfSubscriberExist(email))
            {
                return Json(new JsonViewModel(false, "Email already exists!"));
            }

            this._mailService.Add(recipeSubscription);
            this._mailService.SubscribeRecipes(recipeSubscription.Id, email);

            return Json(new JsonViewModel(true, "You have successfully subscribed!"));
        }

        [Route("[controller]/Recipe/Unsubscribe")]
        public IActionResult UnsubscribeRecipes(long id)
        {
            RecipeSubscription recipeSubscription = this._mailService.GetById(id);
            string message = string.Empty;

            if (recipeSubscription != null)
            {
                this._mailService.UnsubscribeRecipes(recipeSubscription.Email);
                this._mailService.Remove(recipeSubscription);
                message = "You have successfully unsubscribe for recipes.";
            }
            else
            {
                message = "You have already unsubscribe for recipes.";
            }

            ViewData["message"] = message;

            return View("UnsubscribeRecipes");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("Administration/[controller]/Recipe/Send")]
        public JsonResult SendToSubscribers()
        {
            List<Recipe> recipes = this._recipeService.GetByRatingWeek();
            List<RecipeSubscription> subscribers = this._mailService.GetAll();
            this._mailService.SendToSubscribers(recipes, subscribers);

            return Json(new JsonViewModel(true, "You have successfully sent recipes."));
        }
    }
}
