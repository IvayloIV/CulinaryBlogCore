using System.Linq;

using CulinaryBlogCore.Data.Models.Entities;
using CulinaryBlogCore.Services.Contracts;
using CulinaryBlogCore.Services.Repository.Contracts;

using Microsoft.EntityFrameworkCore;

namespace CulinaryBlogCore.Services
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
            this._repository.Add(product);
        }

        public void RemoveById(long id)
        {
            Product product = this.GetById(id);
            this._repository.Delete(product);
        }

        public Product GetById(long id)
        { 
            return this._repository.Set<Product>()
                .Include(r => r.Recipe)
                .Where(p => p.Id == id)
                .FirstOrDefault();
        }

        public bool CheckIfExist(long recipeId, string productName) {
            return this._repository.Set<Product>()
                .Any(c => c.RecipeId == recipeId && c.Name == productName);
        }
    }
}
