using System.Collections.Generic;
using System.Linq;

using CulinaryBlogCore.Data.Models.Entities;
using CulinaryBlogCore.Services.Contracts;
using CulinaryBlogCore.Services.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CulinaryBlogCore.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository _repository;

        public CategoryService(IRepository repository)
        {
            this._repository = repository;
        }

        public List<Category> GetAll()
        {
            return this._repository.Set<Category>()
                .AsNoTracking()
                .OrderBy(c => c.Order)
                .ToList();
        }

        public Category GetById(long id)
        {
            return this._repository.GetById<Category>(id);
        }

        public ILookup<string, ICollection<Recipe>> GetRecipesByCategory()
        {
            return this._repository.Set<Category>()
                .Include(c => c.Recipes)
                .ToLookup(c => c.Name, c => c.Recipes);
        }
    }
}
