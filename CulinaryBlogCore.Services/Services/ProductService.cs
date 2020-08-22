using CulinaryBlogCore.Data.Models.Entities;
using CulinaryBlogCore.Services.Contracts;
using CulinaryBlogCore.Services.Repository.Contracts;
using System;
using System.Linq;

namespace CulinaryBlogCore.Services.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository _repository;

        public ProductService(IRepository repository)
        {
            this._repository = repository;
        }

        public void Add(Product product)
        {
            this._repository.Add<Product>(product);
        }

        public void RemoveById(long id)
        {
            Product product = this.GetById(id);
            this._repository.Delete<Product>(product);
        }

        public Product GetById(long id)
        { 
            return this._repository.GetById<Product>(id);
        }

        public bool CheckIfExist(long recipeId, string productName) {
            int productCount = this._repository.Set<Product>()
                .Where(c => c.RecipeId == recipeId && c.Name == productName)
                .Count();
            return productCount != 0;
        }
    }
}
