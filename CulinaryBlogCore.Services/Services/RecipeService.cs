using CulinaryBlogCore.Data.Models.Entities;
using CulinaryBlogCore.Services.Contracts;
using CulinaryBlogCore.Services.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CulinaryBlogCore.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IRepository _repository;

        public RecipeService(IRepository repository)
        {
            this._repository = repository;
        }

        public void Add(Recipe recipe) {
            recipe.CreationTime = DateTime.Now;
            this._repository.Add<Recipe>(recipe);
        }

        public List<Recipe> GetByRatingWeek() {
            return this._repository.Set<Recipe>()
                .Where(r => r.CreationTime >= DateTime.Now.AddDays(-7) && r.CreationTime <= DateTime.Now)
                .OrderByDescending(r => r.Rating)
                .Take(6)
                .ToList();
        }

        public Recipe GetById(long id) {
            return this._repository.GetById<Recipe>(id);
        }

        public List<Recipe> GetLastAdded()
        {
            return this._repository.Set<Recipe>()
                .OrderByDescending(r => r.CreationTime)
                .Take(3)
                .ToList();
        }
    }
}
