using CulinaryBlogCore.Services.Contracts;

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CulinaryBlogCore.Models.TrickViewModels;
using Microsoft.CodeAnalysis;
using Imgur.API.Endpoints;
using System.Threading.Tasks;
using Imgur.API.Models;
using CulinaryBlogCore.Data.Models.Entities;
using System.Collections.Generic;

namespace CulinaryBlogCore.Controllers
{
    [Authorize]
    public class TrickController : Controller
    {
        private readonly IChefService _chefService;
        private readonly ITrickService _trickService;
        private readonly IMapper _mapper;
        private readonly IImgurTokenService _imgurTokenService;

        public TrickController(
                IChefService chefService,
                ITrickService trickService,
                IMapper mapper,
                IImgurTokenService imgurTokenService)
        {
            this._chefService = chefService;
            this._trickService = trickService;
            this._mapper = mapper;
            this._imgurTokenService = imgurTokenService;
        }

        public IActionResult Index()
        {
            List<Trick> tricks = this._trickService.GetAll();
            List<TrickViewModel> trickViewModel = this._mapper.Map<List<TrickViewModel>>(tricks);
            TrickViewModel trickView = new TrickViewModel
            {
                Tricks = trickViewModel
            };
            return View(trickView);
        }

        [HttpPost]
        public async Task<Trick> Create(CreateTrickViewModel createTrickViewModel) {
            if (createTrickViewModel.Image != null) {
                ImageEndpoint imageEndpoint = await this._imgurTokenService.GetImageEndpoint();
                IImage uploadedImage = await imageEndpoint.UploadImageAsync(createTrickViewModel.Image.OpenReadStream());
                createTrickViewModel.ImageId = uploadedImage.Id;
                createTrickViewModel.ImagePath = uploadedImage.Link;
            }
            Trick trick = this._mapper.Map<Trick>(createTrickViewModel);
            this._trickService.Add(trick);
            return trick;
        }

        [HttpPost]
        public void Update(long id, UpdateTrickViewModel updateTrickViewModel)
        {
            Trick trick = this._mapper.Map<Trick>(updateTrickViewModel);
            this._trickService.Update(id, trick);
        }

        [HttpPost]
        public async Task Delete(long id)
        {
            Trick trick = this._trickService.GetById(id);
            if (trick != null)
            {
                if (trick.ImageId != null) {
                    ImageEndpoint imageEndpoint = await this._imgurTokenService.GetImageEndpoint();
                    await imageEndpoint.DeleteImageAsync(trick.ImageId);
                }
                this._trickService.Delete(trick);
            }
        }
    }
}
