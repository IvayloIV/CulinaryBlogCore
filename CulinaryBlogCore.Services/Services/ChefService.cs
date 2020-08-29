using CulinaryBlogCore.Data.Models.Entities;
using CulinaryBlogCore.Services.Contracts;
using CulinaryBlogCore.Services.Repository.Contracts;
using System;

namespace CulinaryBlogCore.Services.Services
{
    public class ChefService : IChefService
    {
        private readonly IRepository _repository;

        public ChefService(IRepository repository)
        {
            this._repository = repository;
        }

        public void Add(Chef chef)
        {
            chef.CreationTime = DateTime.Now;
            this._repository.Add<Chef>(chef);
        }

    }
}
