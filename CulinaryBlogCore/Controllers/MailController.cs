using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Net.Mime;

using CulinaryBlogCore.Data.Models.Entities;
using CulinaryBlogCore.Data.Models.Identity;
using CulinaryBlogCore.Services.Contracts;
using CulinaryBlogCore.Utils;

using MailKitSmtp = MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;
using Imgur.API.Authentication;
using System.Net.Http;
using Imgur.API.Endpoints;
using Imgur.API.Models;
using Microsoft.AspNetCore.Authorization;

namespace CulinaryBlogCore.Controllers
{
    public class MailController : Controller
    {
        private readonly IMailService _mailService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRecipeService _recipeService;
        private readonly SubscriptionMailData _subscriptionMailData;
        private readonly IHostingEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MailController(
            IMailService mailService,
            IHostingEnvironment env,
            UserManager<ApplicationUser> userManager,
            IRecipeService recipeService,
            IOptions<SubscriptionMailData> subscriptionMailData,
            IHttpContextAccessor httpContextAccessor)
        {
            this._mailService = mailService;
            this._userManager = userManager;
            this._subscriptionMailData = subscriptionMailData.Value;
            this._env = env;
            this._httpContextAccessor = httpContextAccessor;
            this._recipeService = recipeService;
        }

        [HttpPost]
        [Route("[controller]/Recipe/Subscribe")]
        public async Task<string> SubscribeRecipes(string email)
        {
            ApplicationUser user = await this._userManager.GetUserAsync(HttpContext.User);
            RecipeSubscription recipeSubscription = new RecipeSubscription
            {
                Email = email,
                CreationTime = DateTime.Now,
                UserId = user?.Id
            };

            if (email == null || email.Length == 0) {
                return "Email cannot be empty!";
            }

            if (this._mailService.CheckIfSubscriberExist(email)) {
                return "Email already exists!";
            }

            this._mailService.Add(recipeSubscription);

            string subject = "Recipe subscribe";
            string[] lines = System.IO.File.ReadAllLines(Path.Combine(_env.WebRootPath, "templates/subscribe.html"));
            string body = lines.Aggregate((str1, str2) => str1 + Environment.NewLine + str2);

            HttpRequest request = this._httpContextAccessor.HttpContext.Request;
            string baseUrl = $"{request.Scheme}://{request.Host.Value}";
            string unsubLink = $"{baseUrl}/Mail/Recipe/Unsubscribe?id={recipeSubscription.Id}";

            body = body.Replace("{{baseUrl}}", baseUrl);
            body = body.Replace("{{unsubscribelink}}", unsubLink);

            this.SendMail(email, subject, body);
            return "You have successfully subscribed!";
        }

        [Route("[controller]/Recipe/Unsubscribe")]
        public IActionResult UnsubscribeRecipes(long id)
        {
            RecipeSubscription recipeSubscription = this._mailService.GetById(id);
            string message = string.Empty;

            if (recipeSubscription != null)
            {
                string subject = "Recipe unsubscribe";
                string email = recipeSubscription.Email;
                string[] lines = System.IO.File.ReadAllLines(Path.Combine(_env.WebRootPath, "templates/unsubscribe.html"));
                string body = lines.Aggregate((str1, str2) => str1 + Environment.NewLine + str2);

                this.SendMail(email, subject, body);
                this._mailService.Remove(recipeSubscription);
                message = "You have successfully unsubscribe for recipes.";
            } else {
                message = "You have already unsubscribe for recipes.";
            }

            ViewData["message"] = message;
            return View("UnsubscribeRecipes");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("Administration/[controller]/Recipe/Send")]
        public void SendToSubscribers() {
            List<Recipe> recipes = this._recipeService.GetByRatingWeek();
            List<RecipeSubscription> subscribers = this._mailService.GetAll();

            HttpRequest request = this._httpContextAccessor.HttpContext.Request;
            string baseUrl = $"{request.Scheme}://{request.Host.Value}";

            string[] lines = System.IO.File.ReadAllLines(Path.Combine(_env.WebRootPath, "templates/recipe-subscription.html"));
            string body = lines.Aggregate((str1, str2) => str1 + Environment.NewLine + str2);

            lines = System.IO.File.ReadAllLines(Path.Combine(_env.WebRootPath, "templates/recipe-article.html"));
            string recipeTempleate = lines.Aggregate((str1, str2) => str1 + Environment.NewLine + str2);

            string recipeArticles = string.Empty;
            foreach (Recipe recipe in recipes)
            {
                recipeArticles += this.BuildRecipeTemplate(recipe, recipeTempleate, baseUrl);
            }

            string subject = "Best recipes of the week";
            foreach (RecipeSubscription subscriber in subscribers)
            {
                this.SendSubscriberMail(subscriber, recipeArticles, baseUrl, subject, body);
            }
        }

        public void SendMail(string emailTo, string subject, string body) {
            string mailFrom = this._subscriptionMailData.Email;
            var mailMessage = new MailMessage();

            mailMessage.To.Add(new MailAddress(emailTo));
            mailMessage.From = new MailAddress(mailFrom);
            mailMessage.Body = body;
            mailMessage.Subject = subject;

            AlternateView view = AlternateView.CreateAlternateViewFromString(mailMessage.Body, null, MediaTypeNames.Text.Html);
            mailMessage.IsBodyHtml = true;
            mailMessage.AlternateViews.Add(view);

            using (var emailClient = new MailKitSmtp.SmtpClient())
            {
                emailClient.Connect("smtp.abv.bg", 465, true);
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                emailClient.Authenticate(mailFrom, this._subscriptionMailData.Password);
                emailClient.Send((MimeMessage)mailMessage);
                emailClient.Disconnect(true);
            }
        }

        private void SendSubscriberMail(RecipeSubscription subscriber, string recipeArticles, string baseUrl, string subject, string body)
        {
            body = body.Replace("{{recipe-articles}}", recipeArticles)
                       .Replace("{{dateFrom}}", DateTime.Now.AddDays(-7).ToString("dd.MM.yyyy"))
                       .Replace("{{dateTo}}", DateTime.Now.ToString("dd.MM.yyyy"))
                       .Replace("{{unsubscribelink}}", $"{baseUrl}/Mail/Recipe/Unsubscribe?id={subscriber.Id}");

            this.SendMail(subscriber.Email, subject, body);
        }

        private string BuildRecipeTemplate(Recipe recipe, string template, string baseUrl) {
            double rating = recipe.UserRecipeRatings.Sum(ur => ur.Rating) / (double)Math.Max(recipe.UserRecipeRatings.Count, 1);
            string stars = string.Empty;

            for (int i = 1; i <= 5; i++)
            {
                if (i <= rating) {
                    stars += "<span style=\"color: #ffc11c; font-size: 30px;\">★</span>";
                } else {
                    stars += "<span style=\"color: #ffc11c; font-size: 30px;\">☆</span>";
                }
            }

            return template.Replace("{{name}}", recipe.Name)
                    .Replace("{{preparationTime}}", recipe.PreparationTime.ToString("hh:mm"))
                    .Replace("{{cookingTime}}", recipe.CookingTime.ToString("hh:mm"))
                    .Replace("{{portions}}", recipe.Portions.ToString())
                    .Replace("{{imageUrl}}", $"{recipe.ImagePath}")
                    .Replace("{{ratingStars}}", stars)
                    .Replace("{{rating}}", $"{rating:F2}")
                    .Replace("{{voteCount}}", recipe.UserRecipeRatings.Count.ToString())
                    .Replace("{{views}}", recipe.ViewCount.ToString())
                    .Replace("{{description}}", recipe.Description)
                    .Replace("{{detailsLink}}", $"{baseUrl}/Recipe/More/{recipe.Id}");
        }
    }
}
