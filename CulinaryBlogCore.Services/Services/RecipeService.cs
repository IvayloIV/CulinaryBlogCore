using System;
using System.Collections.Generic;
using System.Linq;

using CulinaryBlogCore.Data.Models.Entities;
using CulinaryBlogCore.Services.Contracts;
using CulinaryBlogCore.Services.Repository.Contracts;

using Microsoft.EntityFrameworkCore;

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

        public void UpdateById(long id, Recipe recipe)
        {
            Recipe oldRecipe = this.GetById(id);

            oldRecipe.Name = recipe.Name;
            oldRecipe.Description = recipe.Description;
            oldRecipe.PreparationTime = recipe.PreparationTime;
            oldRecipe.CookingTime = recipe.CookingTime;
            oldRecipe.Portions = recipe.Portions;
            oldRecipe.ImagePath = recipe.ImagePath;
            oldRecipe.CategoryId = recipe.CategoryId;

            this._repository.Update<Recipe>(oldRecipe);
        }

        public void DeleteById(long id) {
            Recipe recipe = this.GetById(id);
            this._repository.Delete<Recipe>(recipe);
        }

        public List<Recipe> GetByRatingWeek() {
        Func<Recipe, double> sumRecipeRating = (r) => r.UserRecipeRatings.Sum(ur => ur.Rating) / (double)Math.Max(r.UserRecipeRatings.Count, 1);
            return this._repository.Set<Recipe>()
                .Include(x => x.UserRecipeRatings)
                .Where(r => r.CreationTime >= DateTime.Now.AddDays(-7) && r.CreationTime <= DateTime.Now)
                .OrderByDescending(sumRecipeRating)
                .Take(6)
                .AsQueryable()
                .AsNoTracking()
                .ToList();
        }

        public Recipe GetById(long id) {
            return this._repository.Set<Recipe>()
                .Include("Category")
                .Include("Products")
                .Include("UserRecipeRatings")
                .Include("User")
                .Where(r => r.Id == id)
                .First();
        }

        public List<Recipe> GetLastAdded()
        {
            return this._repository.Set<Recipe>()
                .Include("UserRecipeRatings")
                .OrderByDescending(r => r.CreationTime)
                .Take(3)
                .ToList();
        }

        public List<Recipe> GetByCategoryId(long categoryId) {
            return this._repository.Set<Recipe>()
                .Include("Products")
                .Include("UserRecipeRatings")
                .Where(r => r.CategoryId == categoryId)
                .ToList();
        }

        public Recipe UpdateByRating(UserRecipeRating recipeRating) {
            this._repository.Add<UserRecipeRating>(recipeRating);
            return this.GetById(recipeRating.RecipeId);
        }

        public List<UserRecipeRating> GetRecipeRatingByUser(string userId) {
            return this._repository.Set<UserRecipeRating>()
                .Where(r => r.UserId == userId)
                .ToList();
        }

        public Recipe UpdateViewCount(long recipeId) {
            Recipe recipe = this.GetById(recipeId);
            recipe.ViewCount += 1;
            this._repository.Update(recipe);
            return recipe;
        }

        public List<Recipe> GetByUserId(string userId)
        {
            return this._repository.Set<Recipe>()
                .Include("UserRecipeRatings")
                .Include("User")
                .Include("Products")
                .Where(r => r.User.Id == userId)
                .OrderByDescending(r => r.CreationTime)
                .ToList();
        }
    }
}
