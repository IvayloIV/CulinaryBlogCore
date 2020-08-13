using CulinaryBlogCore.Data.Models.Entities;
using CulinaryBlogCore.Services.Contracts;
using CulinaryBlogCore.Services.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CulinaryBlogCore.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository _repository;

        public CategoryService(IRepository repository)
        {
            this._repository = repository;
        }


        public List<Category> getAll()
        {
            return this._repository.Set<Category>().ToList();
        }
    }
}
