using System;
using System.Collections.Generic;
using System.Linq;
using CulinaryBlogCore.Data.Models.Entities;
using CulinaryBlogCore.Services.Contracts;
using CulinaryBlogCore.Services.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CulinaryBlogCore.Services
{
    public class TrickService : ITrickService
    {
        private readonly IRepository _repository;

        public TrickService(IRepository repository)
        {
            this._repository = repository;
        }

        public void Add(Trick trick) {
            trick.CreationTime = DateTime.Now;
            this._repository.Add<Trick>(trick);
        }

        public Trick GetById(long id) {
            return this._repository.GetById<Trick>(id);
        }

        public void Delete(Trick trick) {
            this._repository.Delete<Trick>(trick);
        }

        public List<Trick> GetAll() {
            return this._repository.Set<Trick>()
                .Include(t => t.Chef)
                .OrderByDescending(t => t.CreationTime)
                .ToList();
        }

        public void Update(long id, Trick newTrick) {
            Trick trick = this.GetById(id);

            trick.Name = newTrick.Name;
            trick.Description = newTrick.Description;
            trick.ChefId = newTrick.ChefId;

            this._repository.Update<Trick>(trick);
        }
    }
}
