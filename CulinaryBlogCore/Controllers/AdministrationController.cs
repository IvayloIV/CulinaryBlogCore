using System.Collections.Generic;

using CulinaryBlogCore.Data.Models.Entities;
using CulinaryBlogCore.Models.RecipeViewModels;
using CulinaryBlogCore.Services.Contracts;

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CulinaryBlogCore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        private readonly IMailService _mailService;
        private readonly IMapper _mapper;

        public AdministrationController(
           IMailService mailService,
           IMapper mapper)
        {
            this._mailService = mailService;
            this._mapper = mapper;
        }

        [Route("[controller]/Recipe/Subscriptions")]
        public IActionResult RecipeSubscriptions()
        {
            List<RecipeSubscription> subscriptions = this._mailService.GetAll();
            List<RecipeSubscriptionViewModel> subscriptionsModel = this._mapper.Map<List<RecipeSubscriptionViewModel>>(subscriptions);

            RecipeSubscriptionViewModel model = new RecipeSubscriptionViewModel
            {
                Subscriptions = subscriptionsModel
            };

            return View(model);
        }
    }
}
