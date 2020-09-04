using System.Threading.Tasks;
using System.Collections.Generic;

using CulinaryBlogCore.Data.Models.Entities;
using CulinaryBlogCore.Models.ChefViewModels;
using CulinaryBlogCore.Services.Contracts;

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CulinaryBlogCore.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("Administration/[controller]/[action]/{id?}")]
    public class ChefController : Controller
    {
        private readonly IChefService _chefService;
        private readonly IMapper _mapper;
        private readonly IImgurService _imgurService;

        public ChefController(
            IChefService chefService,
            IMapper mapper,
            IImgurService imgurService)
        {
            this._chefService = chefService;
            this._mapper = mapper;
            this._imgurService = imgurService;
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateChefViewModel createChefViewModel)
        {
            if (ModelState.IsValid && createChefViewModel.Image != null)
            {
                await this._imgurService.UploadImage(createChefViewModel);
                this._chefService.Add(this._mapper.Map<Chef>(createChefViewModel));

                return RedirectToAction("Index", "Home");
            }

            if (createChefViewModel.Image == null)
            {
                ModelState.AddModelError("Image", "The Image field is required.");
            }

            return View(createChefViewModel);
        }

        public IActionResult Update(long id)
        {
            Chef chef = this._chefService.GetById(id);
            UpdateChefViewModel model = this._mapper.Map<UpdateChefViewModel>(chef);

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Update(long id, UpdateChefViewModel chefViewModel)
        {
            if (ModelState.IsValid)
            {
                if (chefViewModel.Image != null)
                {
                    await this._imgurService.EditImage(chefViewModel);
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
            DeleteChefViewModel model = this._mapper.Map<DeleteChefViewModel>(chef);

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(long id, string imageId)
        {
            if (imageId != null)
            {
                await this._imgurService.DeleteImage(imageId);
                this._chefService.DeleteById(id);
            }

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public List<ChefViewModel> GetAll()
        {
            List<Chef> chefs = this._chefService.GetAll();
            List<ChefViewModel> model = this._mapper.Map<List<ChefViewModel>>(chefs);

            return model;
        }
    }
}
