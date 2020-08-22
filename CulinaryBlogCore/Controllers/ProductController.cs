using AutoMapper;
using CulinaryBlogCore.Data.Models.Entities;
using CulinaryBlogCore.Models.ProductViewModels;
using CulinaryBlogCore.Services.Contracts;
using CulinaryBlogCore.Services.Services;
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
        private readonly IMapper _mapper;

        public ProductController(
            IProductService productService,
            IRecipeService recipeService,
            IMapper mapper)
        {
            this._productService = productService;
            this._recipeService = recipeService;
            this._mapper = mapper;
        }

        // POST: Product/Create
        [HttpPost]
        public long Create(CreateProductViewModel cpvm)
        {
            Product product = this._mapper.Map<Product>(cpvm);
            if (this._productService.CheckIfExist(cpvm.RecipeId, cpvm.Name)) {
                return -1;
            }
            this._productService.Add(product);
            return product.Id;
        }

        // POST: Product/Delete/Id
        [HttpPost]
        public void Delete(long id)
        {
            this._productService.RemoveById(id);
        }
    }
}
