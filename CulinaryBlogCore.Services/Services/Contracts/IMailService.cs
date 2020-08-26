using System.Collections.Generic;

using CulinaryBlogCore.Data.Models.Entities;

namespace CulinaryBlogCore.Services.Contracts
{
    public interface IMailService
    {
        void Add(RecipeSubscription recipeSubscription);

        bool CheckIfSubscriberExist(string email);

        RecipeSubscription GetById(long id);

        void Remove(RecipeSubscription recipe);

        List<RecipeSubscription> GetAll();
    }
}
