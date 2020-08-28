using System.Collections.Generic;
using System.Linq;

using CulinaryBlogCore.Data.Models.Entities;
using CulinaryBlogCore.Services.Contracts;
using CulinaryBlogCore.Services.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CulinaryBlogCore.Services.Services
{
    public class MailService : IMailService
    {
        private readonly IRepository _repository;

        public MailService(IRepository repository)
        {
            this._repository = repository;
        }

        public void Add(RecipeSubscription recipeSubscription)
        {
            this._repository.Add(recipeSubscription);
        }

        public RecipeSubscription GetById(long id) {
            return this._repository.GetById<RecipeSubscription>(id);
        }

        public List<RecipeSubscription> GetAll() {
            return this._repository.Set<RecipeSubscription>()
                .Include(s => s.User)
                .ToList();
        }

        public void Remove(RecipeSubscription recipe)
        {
            this._repository.Delete<RecipeSubscription>(recipe);
        }

        public bool CheckIfSubscriberExist(string email) {
            int count = this._repository.Set<RecipeSubscription>()
                .Where(s => s.Email == email)
                .Count();

            return count >= 1;
        }
    }
}
