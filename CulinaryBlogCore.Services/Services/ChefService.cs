using System;
using System.Collections.Generic;
using System.Linq;

using CulinaryBlogCore.Data.Models.Entities;
using CulinaryBlogCore.Services.Contracts;
using CulinaryBlogCore.Services.Repository.Contracts;

using Microsoft.EntityFrameworkCore;

namespace CulinaryBlogCore.Services
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
            this._repository.Add(chef);
        }

        public List<Chef> GetAll()
        {
            return this._repository.Set<Chef>()
                .AsNoTracking()
                .OrderByDescending(c => c.CreationTime)
                .ToList();
        }

        public Chef GetById(long id)
        {
            return this._repository.GetById<Chef>(id);
        }

        public void UpdateById(long id, Chef newChef)
        {
            Chef chef = this.GetById(id);

            chef.LastName = newChef.LastName;
            chef.Description = newChef.Description;
            chef.ImagePath = newChef.ImagePath;
            chef.ImageId = newChef.ImageId;

            this._repository.Update(chef);
        }

        public void DeleteById(long id)
        {
            Chef chef = this.GetById(id);
            this._repository.Delete(chef);
        }
    }
}
