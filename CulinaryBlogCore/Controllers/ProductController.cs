using AutoMapper;
using CulinaryBlogCore.Data.Models.Entities;
using CulinaryBlogCore.Data.Models.Identity;
using CulinaryBlogCore.Enums;
using CulinaryBlogCore.Models.ProductViewModels;
using CulinaryBlogCore.Services.Contracts;
using CulinaryBlogCore.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CulinaryBlogCore.Controllers
{
    public class ProductController : Controller
    {
        public readonly IProductService _productService;
        private readonly IRecipeService _recipeService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public ProductController(
            IProductService productService,
            IRecipeService recipeService,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            this._productService = productService;
            this._recipeService = recipeService;
            this._mapper = mapper;
            this._userManager = userManager;
        }

        // POST: Product/Create
        [HttpPost]
        [Authorize]
        public async Task<long> Create(CreateProductViewModel cpvm)
        {
            Product product = this._mapper.Map<Product>(cpvm);
            if (this._productService.CheckIfExist(cpvm.RecipeId, cpvm.Name)) {
                return -1;
            }

            Recipe recipe = this._recipeService.GetById(product.RecipeId);
            if (!(await this.IsAdminOrOwner(() => recipe.UserId))) {
                return -1;
            }

            this._productService.Add(product);
            return product.Id;
        }

        // POST: Product/Delete/Id
        [HttpPost]
        [Authorize]
        public async Task Delete(long id)
        {
            Product product = this._productService.GetById(id);
            if (await this.IsAdminOrOwner(() => product.Recipe.UserId)) {
                this._productService.RemoveById(id);
            }
        }

        private async Task<bool> IsAdminOrOwner(Func<string> getUserId)
        {
            ApplicationUser user = await this._userManager.GetUserAsync(HttpContext.User);
            IList<string> roles = await _userManager.GetRolesAsync(user);

            return roles.Any(r => r == Role.Admin.ToString()) || user.Id == getUserId.Invoke();
        }
    }
}
