using System.Collections.Generic;
using System.Threading.Tasks;

using CulinaryBlogCore.Services.Contracts;
using CulinaryBlogCore.Models.TrickViewModels;
using CulinaryBlogCore.Data.Models.Entities;
using CulinaryBlogCore.Data.Models.Identity;

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace CulinaryBlogCore.Controllers
{
    [Authorize]
    public class TrickController : BaseController
    {
        private readonly ITrickService _trickService;
        private readonly IMapper _mapper;
        private readonly IImgurService _imgurService;

        public TrickController(
            ITrickService trickService,
            IMapper mapper,
            IImgurService imgurService,
            UserManager<ApplicationUser> userManager)
            : base(userManager)
        {
            this._trickService = trickService;
            this._mapper = mapper;
            this._imgurService = imgurService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await base._userManager.GetUserAsync(HttpContext.User);
            List<Trick> tricks = this._trickService.GetAll();

            TrickViewModel model = new TrickViewModel
            {
                Tricks = this._mapper.Map<List<TrickViewModel>>(tricks),
                CurrentUserId = user?.Id
            };

            return View(model);
        }

        [HttpPost]
        public async Task<Trick> Create(CreateTrickViewModel createTrickViewModel)
        {
            if (createTrickViewModel.Image != null)
            {
                await this._imgurService.UploadImage(createTrickViewModel);
            }

            ApplicationUser user = await base._userManager.GetUserAsync(HttpContext.User);
            Trick trick = this._mapper.Map<Trick>(createTrickViewModel);
            trick.UserId = user.Id;
            this._trickService.Add(trick);

            return trick;
        }

        [HttpPost]
        public async Task<Trick> Update(long id, UpdateTrickViewModel updateTrickViewModel)
        {
            Trick oldTrick = this._trickService.GetById(id);

            if (oldTrick != null && await base.IsAdminOrOwner(oldTrick.UserId)) 
            {
                Trick newTrick = this._mapper.Map<Trick>(updateTrickViewModel);
                this._trickService.Update(oldTrick, newTrick);
            }
            
            return oldTrick;
        }

        [HttpPost]
        public async Task Delete(long id)
        {
            Trick trick = this._trickService.GetById(id);

            if (trick != null && await base.IsAdminOrOwner(trick.UserId))
            {
                if (trick.ImageId != null)
                {
                    await this._imgurService.DeleteImage(trick.ImageId);
                }

                this._trickService.Delete(trick);
            }
        }
    }
}
