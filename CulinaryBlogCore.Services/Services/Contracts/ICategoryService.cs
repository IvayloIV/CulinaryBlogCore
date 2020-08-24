using System.Collections.Generic;

using CulinaryBlogCore.Data.Models.Entities;

namespace CulinaryBlogCore.Services.Contracts
{
    public interface ICategoryService
    {
        List<Category> GetAll();

        Category GetById(long id);
    }
}
