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
        private readonly IMapper _mapper;

        public ProductController(IProductService productService,
            IMapper mapper)
        {
            this._productService = productService;
            this._mapper = mapper;
        }

        // POST: Product/Create
        [HttpPost]
        public long Create(CreateProductViewModel createProductViewModel)
        {
            Product product = this._mapper.Map<Product>(createProductViewModel);
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
