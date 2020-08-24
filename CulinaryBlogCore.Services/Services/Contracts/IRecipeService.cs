using CulinaryBlogCore.Data.Models.Entities;
using System.Collections.Generic;

namespace CulinaryBlogCore.Services.Contracts
{
    public interface IRecipeService
    {
        void Add(Recipe recipe);

        void UpdateById(long id, Recipe recipe);

        void DeleteById(long id);

        List<Recipe> GetByRatingWeek();

        Recipe GetById(long id);

        List<Recipe> GetLastAdded();

        List<Recipe> GetByCategoryId(long categoryId);

        Recipe UpdateByRating(UserRecipeRating recipeRating);

        List<UserRecipeRating> GetRecipeRatingByUser(string userId);

        Recipe UpdateViewCount(long recipeId);

        List<Recipe> GetByUserId(string userId);
    }
}
