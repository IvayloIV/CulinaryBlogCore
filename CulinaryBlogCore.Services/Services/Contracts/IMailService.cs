using System.Collections.Generic;

using CulinaryBlogCore.Data.Models.Entities;

namespace CulinaryBlogCore.Services.Contracts
{
    public interface IMailService
    {
        void Add(RecipeSubscription recipeSubscription);

        bool CheckIfSubscriberExist(string email);

        RecipeSubscription GetById(long id);

        void Remove(RecipeSubscription recipeSubscription);

        List<RecipeSubscription> GetAll();

        void SubscribeRecipes(long subscriptionId, string email);

        void UnsubscribeRecipes(string email);

        void SendToSubscribers(List<Recipe> recipes, List<RecipeSubscription> subscribers);
    }
}
