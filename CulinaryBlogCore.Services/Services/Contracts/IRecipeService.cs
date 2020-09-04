using System.Collections.Generic;

using CulinaryBlogCore.Data.Models.Entities;

namespace CulinaryBlogCore.Services.Contracts
{
    public interface IRecipeService
    {
        void Add(Recipe recipe);

        void UpdateById(long id, Recipe recipe);

        void DeleteById(long id);

        List<Recipe> GetByRatingWeek();

        double CalculateRating(Recipe recipe);

        Recipe GetById(long id);

        List<Recipe> GetLastAdded();

        List<Recipe> GetByCategoryId(long categoryId);

        Recipe UpdateByRating(UserRecipeRating recipeRating);

        List<UserRecipeRating> GetRecipeRatingByUser(string userId);

        Recipe UpdateViewCount(long recipeId);

        List<Recipe> GetByUserId(string userId);

        void CalculateRecipesRating(List<Recipe> recipes);

        void CalculateRecipeRating(Recipe recipe);

        void CalculateUserVotes(List<Recipe> recipes, string userId);

        void CalculateUserVote(Recipe recipe, string userId);
    }
}
