using CulinaryBlogCore.Data.Models.Entities;
using System.Collections.Generic;

namespace CulinaryBlogCore.Services.Contracts
{
    public interface IRecipeService
    {
        void Add(Recipe recipe);

        List<Recipe> GetByRatingWeek();

        Recipe GetById(long id);

        List<Recipe> GetLastAdded();
    }
}
