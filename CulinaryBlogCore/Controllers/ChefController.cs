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
    [Route("Administration/[controller]/[action]/{id?}")]
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
        public async Task<ActionResult> Create(CreateChefViewModel createChefViewModel)
        {
            if (ModelState.IsValid)
            {
                ImageEndpoint imageEndpoint = await this._imgurTokenService.GetImageEndpoint();
                IImage imageData = await imageEndpoint.UploadImageAsync(createChefViewModel.Image.OpenReadStream());
                createChefViewModel.ImagePath = imageData.Link;
                createChefViewModel.ImageId = imageData.Id;

                this._chefService.Add(this._mapper.Map<Chef>(createChefViewModel));
                return RedirectToAction("Index", "Home");
            }
            return View(createChefViewModel);
        }

        public IActionResult Update(long id)
        {
            Chef chef = this._chefService.GetById(id);
            UpdateChefViewModel updateViewModel = this._mapper.Map<UpdateChefViewModel>(chef);
            return View(updateViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Update(long id, UpdateChefViewModel chefViewModel)
        {
            if (ModelState.IsValid)
            {
                if (chefViewModel.Image != null)
                {
                    ImageEndpoint imageEndpoint = await this._imgurTokenService.GetImageEndpoint();
                    await imageEndpoint.DeleteImageAsync(chefViewModel.ImageId);
                    IImage uploadedImage = await imageEndpoint.UploadImageAsync(chefViewModel.Image.OpenReadStream());
                    chefViewModel.ImageId = uploadedImage.Id;
                    chefViewModel.ImagePath = uploadedImage.Link;
                }

                this._chefService.UpdateById(id, this._mapper.Map<Chef>(chefViewModel));
                return RedirectToAction("Index", "Home");
            }
            chefViewModel.Id = id;
            return View(chefViewModel);
        }

        public IActionResult Delete(long id)
        {
            Chef chef = this._chefService.GetById(id);
            DeleteChefViewModel deleteViewModel = this._mapper.Map<DeleteChefViewModel>(chef);
            return View(deleteViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(long id, string imageId)
        {
            if (imageId != null)
            {
                ImageEndpoint imageEndpoint = await this._imgurTokenService.GetImageEndpoint();
                await imageEndpoint.DeleteImageAsync(imageId);
                this._chefService.DeleteById(id);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
