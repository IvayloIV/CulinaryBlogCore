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

        public void Add(Recipe recipe)
        {
            recipe.CreationTime = DateTime.Now;
            this._repository.Add(recipe);
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
            oldRecipe.ImageId = recipe.ImageId;
            oldRecipe.CategoryId = recipe.CategoryId;

            this._repository.Update(oldRecipe);
        }

        public void DeleteById(long id)
        {
            Recipe recipe = this.GetById(id);
            this._repository.Delete(recipe);
        }

        public List<Recipe> GetByRatingWeek()
        {
            return this._repository.Set<Recipe>()
                .Include(x => x.UserRecipeRatings)
                .Where(r => r.CreationTime >= DateTime.Now.AddDays(-7) && r.CreationTime <= DateTime.Now)
                .AsNoTracking()
                .OrderByDescending(CalculateRating)
                .Take(6)
                .ToList();
        }

        public double CalculateRating(Recipe recipe) {
            return recipe.UserRecipeRatings.Sum(ur => ur.Rating) / (double)Math.Max(recipe.UserRecipeRatings.Count, 1);
        }

        public Recipe GetById(long id)
        {
            return this._repository.Set<Recipe>()
                .Include(r => r.Category)
                .Include(r => r.Products)
                .Include(r => r.UserRecipeRatings)
                .Include(r => r.User)
                .Where(r => r.Id == id)
                .FirstOrDefault();
        }

        public List<Recipe> GetLastAdded()
        {
            return this._repository.Set<Recipe>()
                .Include(r => r.UserRecipeRatings)
                .AsNoTracking()
                .OrderByDescending(r => r.CreationTime)
                .Take(3)
                .ToList();
        }

        public List<Recipe> GetByCategoryId(long categoryId)
        {
            return this._repository.Set<Recipe>()
                .Include(c => c.Products)
                .Include(c => c.UserRecipeRatings)
                .Where(r => r.CategoryId == categoryId)
                .AsNoTracking()
                .ToList();
        }

        public Recipe UpdateByRating(UserRecipeRating recipeRating)
        {
            this._repository.Add(recipeRating);
            return this.GetById(recipeRating.RecipeId);
        }

        public List<UserRecipeRating> GetRecipeRatingByUser(string userId)
        {
            return this._repository.Set<UserRecipeRating>()
                .Where(r => r.UserId == userId)
                .ToList();
        }

        public Recipe UpdateViewCount(long recipeId)
        {
            Recipe recipe = this.GetById(recipeId);
            recipe.ViewCount += 1;
            this._repository.Update(recipe);

            return recipe;
        }

        public List<Recipe> GetByUserId(string userId)
        {
            return this._repository.Set<Recipe>()
                .Include(r => r.UserRecipeRatings)
                .Include(r => r.User)
                .Include(r => r.Products)
                .Where(r => r.User.Id == userId)
                .AsNoTracking()
                .OrderByDescending(r => r.CreationTime)
                .ToList();
        }

        public void CalculateRecipesRating(List<Recipe> recipes)
        {
            for (int i = 0; i < recipes.Count; i++)
            {
                Recipe currRecipe = recipes[i];
                this.CalculateRecipeRating(currRecipe);
            }
        }

        public void CalculateRecipeRating(Recipe recipe)
        {
            double rating = this.CalculateRating(recipe);
            recipe.Rating = $"{rating:F2}";
            recipe.VoteCount = recipe.UserRecipeRatings.Count;
        }

        public void CalculateUserVotes(List<Recipe> recipes, string userId)
        {
            for (int i = 0; i < recipes.Count; i++)
            {
                Recipe currRecipe = recipes[i];
                this.CalculateUserVote(currRecipe, userId);
            }
        }

        public void CalculateUserVote(Recipe recipe, string userId)
        {
            UserRecipeRating userRecipeRating = recipe.UserRecipeRatings.FirstOrDefault(ur => ur.UserId == userId);
            recipe.UserRating = userRecipeRating != null ? userRecipeRating.Rating : 0;
        }
    }
}
