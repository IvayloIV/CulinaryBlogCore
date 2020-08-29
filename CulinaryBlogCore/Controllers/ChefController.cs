using System.Threading.Tasks;

using CulinaryBlogCore.Data.Models.Entities;
using CulinaryBlogCore.Models.ChefViewModels;
using CulinaryBlogCore.Services.Contracts;

using AutoMapper;
using Imgur.API.Endpoints;
using Imgur.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CulinaryBlogCore.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("Administration/[controller]/[action]")]
    public class ChefController : Controller
    {
        private readonly IChefService _chefService;
        private readonly IMapper _mapper;
        private readonly IImgurTokenService _imgurTokenService;

        public ChefController(
                IChefService chefService,
                IMapper mapper,
                IImgurTokenService imgurTokenService)
        {
            this._chefService = chefService;
            this._mapper = mapper;
            this._imgurTokenService = imgurTokenService;
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateChefViewModel createChefViewModel, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                ImageEndpoint imageEndpoint = await this._imgurTokenService.GetImageEndpoint();
                IImage imageData = await imageEndpoint.UploadImageAsync(image.OpenReadStream());
                createChefViewModel.ImagePath = imageData.Link;
                createChefViewModel.ImageId = imageData.Id;

                this._chefService.Add(this._mapper.Map<Chef>(createChefViewModel));
                return RedirectToAction("Index", "Home");
            }
            return View(createChefViewModel);
        }
    }
}
