using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;

using CulinaryBlogCore.Data.Models.Entities;
using CulinaryBlogCore.Services.Contracts;
using CulinaryBlogCore.Services.Repository.Contracts;
using CulinaryBlogCore.Services.Utils;

using MailKitSmtp = MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;
using Microsoft.EntityFrameworkCore;

namespace CulinaryBlogCore.Services
{
    public class MailService : IMailService
    {
        private readonly IRepository _repository;
        private readonly SubscriptionMailData _subscriptionMailData;
        private readonly IHostingEnvironment _env;
        private readonly IRecipeService _recipeService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MailService(
            IRepository repository,
            IOptions<SubscriptionMailData> subscriptionMailData,
            IHostingEnvironment env,
            IRecipeService recipeService,
            IHttpContextAccessor httpContextAccessor)
        {
            this._repository = repository;
            this._subscriptionMailData = subscriptionMailData.Value;
            this._env = env;
            this._recipeService = recipeService;
            this._httpContextAccessor = httpContextAccessor;
        }

        public void Add(RecipeSubscription recipeSubscription)
        {
            this._repository.Add(recipeSubscription);
        }

        public RecipeSubscription GetById(long id)
        {
            return this._repository.GetById<RecipeSubscription>(id);
        }

        public List<RecipeSubscription> GetAll()
        {
            return this._repository.Set<RecipeSubscription>()
                .AsNoTracking()
                .Include(s => s.User)
                .ToList();
        }

        public void Remove(RecipeSubscription recipeSubscription)
        {
            this._repository.Delete(recipeSubscription);
        }

        public bool CheckIfSubscriberExist(string email)
        {
            return this._repository.Set<RecipeSubscription>()
                .Any(s => s.Email == email);
        }

        public void SubscribeRecipes(long subscriptionId, string email)
        {
            string subject = "Recipe subscribe";
            string body = this.ReadTemplate("subscribe");

            string baseUrl = this.GetBaseUrl();
            string unsubLink = $"{baseUrl}/Mail/Recipe/Unsubscribe?id={subscriptionId}";

            body = body.Replace("{{baseUrl}}", baseUrl);
            body = body.Replace("{{unsubscribelink}}", unsubLink);

            this.SendMail(email, subject, body);
        }

        public void UnsubscribeRecipes(string email)
        {
            string subject = "Recipe unsubscribe";
            string body = this.ReadTemplate("unsubscribe");

            this.SendMail(email, subject, body);
        }

        public void SendToSubscribers(List<Recipe> recipes, List<RecipeSubscription> subscribers)
        {
            string baseUrl = this.GetBaseUrl();
            string body = this.ReadTemplate("recipe-subscription");
            string recipeTempleate = this.ReadTemplate("recipe-article");

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

        private string ReadTemplate(string template)
        {
            string[] lines = File.ReadAllLines(Path.Combine(_env.WebRootPath, $"templates/{template}.html"));
            return lines.Aggregate((str1, str2) => str1 + Environment.NewLine + str2);
        }

        private string GetBaseUrl()
        {
            HttpRequest request = this._httpContextAccessor.HttpContext.Request;
            return $"{request.Scheme}://{request.Host.Value}";
        }

        private void SendMail(string emailTo, string subject, string body)
        {
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

        private string BuildRecipeTemplate(Recipe recipe, string template, string baseUrl)
        {
            double rating = this._recipeService.CalculateRating(recipe);
            string stars = string.Empty;

            for (int i = 1; i <= 5; i++)
            {
                if (i <= rating)
                {
                    stars += "<span style=\"color: #ffc11c; font-size: 30px;\">★</span>";
                }
                else
                {
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
