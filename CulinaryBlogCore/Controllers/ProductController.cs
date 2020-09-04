using System.Threading.Tasks;

using CulinaryBlogCore.Data.Models.Entities;
using CulinaryBlogCore.Data.Models.Identity;
using CulinaryBlogCore.Models.ProductViewModels;
using CulinaryBlogCore.Services.Contracts;

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CulinaryBlogCore.Models.ResponseViewModel;

namespace CulinaryBlogCore.Controllers
{
    [Authorize]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IRecipeService _recipeService;
        private readonly IMapper _mapper;

        public ProductController(
            IProductService productService,
            IRecipeService recipeService,
            IMapper mapper,
            UserManager<ApplicationUser> userManager) : base(userManager)
        {
            this._productService = productService;
            this._recipeService = recipeService;
            this._mapper = mapper;
        }

        [HttpPost]
        public async Task<JsonResult> Create(CreateProductViewModel productModel)
        {
            Product product = this._mapper.Map<Product>(productModel);

            if (this._productService.CheckIfExist(productModel.RecipeId, productModel.Name)) {
                return Json(new JsonViewModel(false, "Product already exists!"));
            }

            Recipe recipe = this._recipeService.GetById(product.RecipeId);
            if (!(await base.IsAdminOrOwner(recipe.UserId))) {
                return Json(new JsonViewModel(false, "You don't have permission!"));
            }

            this._productService.Add(product);
            return Json(new CreateProductJsonViewModel(true, "Successful product creation!", product.Id));
        }

        [HttpPost]
        public async Task<JsonResult> Delete(long id)
        {
            Product product = this._productService.GetById(id);
            if (await base.IsAdminOrOwner(product.Recipe.UserId)) {
                this._productService.RemoveById(id);
                return Json(new JsonViewModel(true, "Product successfully removed!"));
            }

            return Json(new JsonViewModel(false, "You don't have permission!"));
        }
    }
}
